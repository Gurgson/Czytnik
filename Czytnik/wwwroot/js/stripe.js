(function () {
  const storage = window.localStorage;
  const urlSearchParams = new URLSearchParams(window.location.search);
  const params = Object.fromEntries(urlSearchParams.entries());

  // The items the customer wants to buy
  let items = ""
  if(storage.getItem('type') == 'single'){
    items = storage.getItem('singleItem');
  }else{
    items = storage.getItem('cartItems');
  }

  let productsPrice = 0;

  if (storage.getItem('type') == 'single') {
    productsPrice = storage.getItem('singleFull').replace(',', '.') * 1;
  } else {
    productsPrice = storage.getItem('cartFull').replace(',', '.') * 1;
  }

  let shipmentPrice = 0;
  let fullPrice = productsPrice + shipmentPrice;
  let shipKey = 'point1';

  const stripe = Stripe(
    'pk_test_51KPASKGvoTqJgR6RzCdDmq39pRUnOI2Eq1LuzdenzeylvGoPEKAwq6zHq8UViH1kxuPPnWYA6ufgk461eGrFFNjw00p3EdlduX'
  );

  let key = '';

  let elements;

  initialize();

  document.querySelector('#payment-form').addEventListener('submit', handleSubmit);

  // Fetches a payment intent and captures the client secret
  function initialize() {
    const shipment = new Map();
    shipment.set('point1', 0);
    shipment.set('point2', 0);
    shipment.set('shipment1', 10.95);
    shipment.set('shipment2', 12.99);
    shipment.set('shipment3', 16.99);

    document.querySelector('.js-checkout-products-price').innerText = `${productsPrice.toFixed(2)} zł`;
    document.querySelector('.js-checkout-shipment-price').innerText = `${shipmentPrice.toFixed(2)} zł`;
    document.querySelector('.js-checkout-full-price').innerText = `${fullPrice.toFixed(2)} zł`;

    document.querySelector('.js-checkout-delivery').addEventListener('click', event => {
      if (event.target.matches('.js-checkout-radio') || event.target.matches('.js-checkout-label')) {
        shipKey = document.querySelector('.js-checkout-radio:checked').value;
        shipmentPrice = shipment.get(shipKey) || 0;
        fullPrice = productsPrice + shipmentPrice;
        document.querySelector('.js-checkout-shipment-price').innerText = `${shipmentPrice.toFixed(2)} zł`;
        document.querySelector('.js-checkout-full-price').innerText = `${fullPrice.toFixed(2)} zł`;
      }
    });

    $.ajax({
      url: '/Checkout/Session',
      type: 'POST',
      data: { products: items },
      datatype: 'json',
      success: function (data) {
        const { clientSecret } = data;
        key = data.key;

        const appearance = {
          theme: 'stripe',
        };
        elements = stripe.elements({ appearance, clientSecret });

        const paymentElement = elements.create('payment');
        paymentElement.mount('#payment-element');
      },
    });
  }

  async function handleSubmit(e) {
    e.preventDefault();
    setLoading(true);

    $.ajax({
      url: '/Checkout/Update',
      type: 'PATCH',
      data: { shipping: shipKey, key: key, products: items, type: storage.getItem('type')},
      datatype: 'json',
      success: async function (data) {
        console.log(data);
        if (data.state == 'success') {
          const { error } = await stripe.confirmPayment({
            elements,
            confirmParams: {
              // Make sure to change this to your payment completion page
              return_url: 'https://localhost:5001/Checkout/Success',
            },
          });

          // This point will only be reached if there is an immediate error when
          // confirming the payment. Otherwise, your customer will be redirected to
          // your `return_url`. For some payment methods like iDEAL, your customer will
          // be redirected to an intermediate site first to authorize the payment, then
          // redirected to the `return_url`.

          if (error.type === 'card_error' || error.type === 'validation_error') {
            showMessage(error.message);
          } else {
            showMessage('An unexpected error occured.');
          }

          setLoading(false);
        }
      },
    });
  }

  // Fetches the payment intent status after payment submission
  // async function checkStatus() {
  //   const clientSecret = new URLSearchParams(window.location.search).get('payment_intent_client_secret');

  //   if (!clientSecret) {
  //     return;
  //   }

  //   const { paymentIntent } = await stripe.retrievePaymentIntent(clientSecret);

  //   switch (paymentIntent.status) {
  //     case 'succeeded':
  //       showMessage('Payment succeeded!');
  //       break;
  //     case 'processing':
  //       showMessage('Your payment is processing.');
  //       break;
  //     case 'requires_payment_method':
  //       showMessage('Your payment was not successful, please try again.');
  //       break;
  //     default:
  //       showMessage('Something went wrong.');
  //       break;
  //   }
  // }

  // // ------- UI helpers -------

  // function showMessage(messageText) {
  //   // const messageContainer = document.querySelector('#payment-message');
  //   // messageContainer.classList.remove('hidden');
  //   // messageContainer.textContent = messageText;
  //   // setTimeout(function () {
  //   //   messageContainer.classList.add('hidden');
  //   //   messageText.textContent = '';
  //   // }, 4000);
  // }

  // Show a spinner on payment submission
  function setLoading(isLoading) {
    if (isLoading) {
      // Disable the button and show a spinner
      document.querySelector('#submit').disabled = true;
      // document.querySelector('#spinner').classList.remove('spinner--hidden');
      // document.querySelector('#button-text').classList.add('hidden');
    } else {
      document.querySelector('#submit').disabled = false;
      // document.querySelector('#spinner').classList.add('spinner--hidden');
      // document.querySelector('#button-text').classList.remove('hidden');
    }
  }
})();
