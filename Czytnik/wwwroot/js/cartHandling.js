(async function () {
  const myStorage = window.localStorage;

    const isUserLogged = document.querySelector('.js-navigation-cart').dataset.logged;

  document.querySelector('.js-checkout-button').addEventListener('click', e => {
    e.preventDefault();
    if (isUserLogged == "False" && !myStorage.getItem('cartItems')) return;

    myStorage.setItem('type', 'cart');

    window.location.href = '/Checkout';
  });


  if (isUserLogged != 'True') {
    await renderCartItemsFromLocalStorage();
  }

  setCartPrice();

  const quantityInputs = document.querySelectorAll('.js-cart-quantity-input');

  quantityInputs.forEach(input => {
    input.addEventListener('change', (e) => {

      if (this.value <= 1) return;
      const cartItem = e.target.closest('.js-cart-item');
      const price = parseFloat(cartItem.querySelector('.js-cart-item-price').innerText.replace(',', '.'));
      const promotionItem = cartItem.querySelector('.js-cart-item-promotion');
      const defaultPromotion = promotionItem.dataset.defaultDiscount.replace(',', '.');

      const fullPrice = calculateFullPrice(price, e.target.value);
      const newPromotion = calculateCartItemPromotion(defaultPromotion, e.target.value);
      const cartFullPriceItem = cartItem.querySelector('.js-cart-item-full-price');
      promotionItem.dataset.discount = newPromotion;
      cartFullPriceItem.innerText = `${fullPrice} zł`;
      setCartPrice();
      setCartPromotion();

    })
  })

  quantityInputs.forEach(input => {
    input.addEventListener('focusout', e => {
      const book = e.target.dataset.book;
      const quantity = e.target.value;

      if (isUserLogged == 'True') updateRecord(book, quantity);
      else {
        let items = JSON.parse(myStorage.getItem('cartItems'));
        if (!items) return;
        items = items.map(item => {
          if (item.bookId == book) {
            return {
              ...item,
              Quantity: parseInt(quantity),
            };
          }

          return item;
        });

        myStorage.setItem('cartItems', JSON.stringify(items));
        deleteButtonClicked = false;
      }
    });
  });

  const cart = document.querySelector('.js-cart-container');
  setCartPromotion();

  cart.addEventListener('click', e => {
    const incrementButton = e.target.closest('.js-cart-quantity-increment');
    const decrementButton = e.target.closest('.js-cart-quantity-decrement');
    const deleteButton = e.target.closest('.js-cart-item-delete');

    if (incrementButton) incrementQuantity(incrementButton);
    else if (decrementButton) decrementQuantity(decrementButton);
    else if (deleteButton) deleteItem(deleteButton);
  });

  let deleteButtonClicked = false;

  const deleteItem = button => {
    if (deleteButtonClicked) return;
    deleteButtonClicked = true;
    const book = button.dataset.book;

    if (isUserLogged == 'True') {
      deleteRecord(button, book);
    }else {
      let items = JSON.parse(myStorage.getItem('cartItems'));
      if (!items) return;
      items = items.filter(item => item.bookId != book);

      myStorage.setItem('cartItems', JSON.stringify(items));
      renderCartItemsFromLocalStorage();
      deleteButtonClicked = false;
        updateNavigationCart();
      if(items.length == 0){
        window.location.href = '/Cart/Empty';
      }
    }
  };

  const decrementQuantity = button => {
    const quantityInputItem = button.nextElementSibling;

    if (quantityInputItem.value <= 1) return;
    quantityInputItem.focus();
    quantityInputItem.value--;
    const cartItem = button.closest('.js-cart-item');
    const price = parseFloat(cartItem.querySelector('.js-cart-item-price').innerText.replace(',', '.'));
    const promotionItem = cartItem.querySelector('.js-cart-item-promotion');
    const defaultPromotion = promotionItem.dataset.defaultDiscount.replace(',', '.');

    const fullPrice = calculateFullPrice(price, quantityInputItem.value);
    const newPromotion = calculateCartItemPromotion(defaultPromotion, quantityInputItem.value);
    const cartFullPriceItem = cartItem.querySelector('.js-cart-item-full-price');
    promotionItem.dataset.discount = newPromotion;
    cartFullPriceItem.innerText = `${fullPrice} zł`;
    setCartPrice();
    setCartPromotion();
  };

  const incrementQuantity = button => {
    const quantityInputItem = button.previousElementSibling;

    quantityInputItem.focus();
    quantityInputItem.value++;
    const cartItem = button.closest('.js-cart-item');
    const price = parseFloat(cartItem.querySelector('.js-cart-item-price').innerText.replace(',', '.'));
    const promotionItem = cartItem.querySelector('.js-cart-item-promotion');
    const defaultPromotion = promotionItem.dataset.defaultDiscount.replace(',', '.');

    const fullPrice = calculateFullPrice(price, quantityInputItem.value);
    const newPromotion = calculateCartItemPromotion(defaultPromotion, quantityInputItem.value);
    const cartFullPriceItem = cartItem.querySelector('.js-cart-item-full-price');
    promotionItem.dataset.discount = newPromotion;
    cartFullPriceItem.innerText = `${fullPrice} zł`;
    setCartPrice();
    setCartPromotion();
  };

  const calculateFullPrice = (price, quantity) => {
    return `${(Math.round(price * quantity * 100) / 100).toFixed(2)}`.replace('.', ',');
  };

  const calculateCartItemPromotion = (discount, quantity) => {
    return Math.round(discount * quantity * 100) / 100;
  };

  function setCartPrice() {
    const prices = Array.from(document.querySelectorAll('.js-cart-item-full-price')).map(el =>
      parseFloat(el.innerText.replace(',', '.'))
    );
    const sum = `${(Math.round(prices.reduce((sum, price) => sum + price, 0) * 100) / 100).toFixed(2)}`.replace(
      '.',
      ','
    );

    myStorage.setItem('cartFull', sum);
    document.querySelector('.js-cart-price').innerText = `${sum} zł`;
  }

  async function getBooks() {
    let booksId = JSON.parse(myStorage.getItem('cartItems'));
    booksId = booksId.map(item => item.bookId).join(',');

    let result = await $.ajax({
      contentType: 'application/json; charset=utf-8',
      dataType: 'json',
      type: 'GET',
      url: '/Cart/GetBooks',
      data: { booksId: booksId },
      error: function (err) {
        console.log(err);
      },
    });

    return result;
  }

  function setCartPromotion() {
    const promotions = Array.from(document.querySelectorAll('.js-cart-item-promotion')).map(el =>
      parseFloat(el.dataset.discount.replace(',', '.'))
    );
    const sum = `${(Math.round(promotions.reduce((sum, promotion) => sum + promotion, 0) * 100) / 100).toFixed(
      2
    )}`.replace('.', ',');

    document.querySelector('.js-cart-promotion').innerText = `-${sum} zł`;
  }

  async function renderCartItemsFromLocalStorage() {
    const books = JSON.parse(await getBooks());
    const items = JSON.parse(myStorage.getItem('cartItems'));

    let booksItems = [];
    for (let i = 0; i < books.length; i++) {
      booksItems.push({
        ...books[i],
        ...items.find(inner => inner.bookId == books[i].bookId),
      });
    }

    document.querySelector('.js-cart-quantity').innerText = `${booksItems.length} przedmioty`;

    let template = `
            <div class="cart__item cart__item-header">
                <div class="cart__item-description cart__item-description--product">Produkt</div>
                <div class="cart__item-description">Ilość</div>
                <div class="cart__item-description">Cena</div>
                <div class="cart__item-description">Razem</div>
            </div>
        `;

    booksItems.forEach(book => {
      template += `
            <div class="cart__item js-cart-item">
                <div class="cart__item-product">
                  <a href="/Book/Index/${book.bookId}" class="cart__item-cover">
                    <img class="cart__item-image" src="${book.Cover}" alt="${book.Title}">
                  </a>
                  <div class="cart__item-info">
                    <div class="cart__item-title" title="${book.Title}">${book.Title}</div>
                    <div class="cart__item-author">${book.Authors.join(', ')}</div>
                    <button class="cart__item-delete js-cart-item-delete" data-book="${book.bookId}">USUŃ</button>
                  </div>
                </div>

                <div class="cart__item-quantity">
                    <button class="cart__item-button js-cart-quantity-decrement">
                    <svg class="cart__item-button-icon" viewBox="0 0 24 24">
                        <use xlink:href="/assets/svg/sprite.svg#icon-minus"></use>
                    </svg>
                    </button>
                    <input class="cart__item-input js-cart-quantity-input" type="number" min="1" name="cartItemQuantity"
                    value="${book.Quantity}" data-book="${book.bookId}">
                    <button class="cart__item-button js-cart-quantity-increment">
                    <svg class="cart__item-button-icon" viewBox="0 0 24 24">
                        <use xlink:href="/assets/svg/sprite.svg#icon-plus"></use>
                    </svg>
                    </button>
                </div>

                <div class="cart__item-price">
                    <div class="cart__item-price-old js-cart-item-promotion" data-default-discount="${
                      book.Price == book.CalculatedPrice ? 0 : book.Price - book.CalculatedPrice
                    }" data-discount="${
        book.Price == book.CalculatedPrice ? 0 : (book.Price - book.CalculatedPrice) * book.Quantity
      }">${book.Price == book.CalculatedPrice ? '' : book.Price + 'zł'} </div>
                    <div class="cart__item-price-new js-cart-item-price">${book.CalculatedPrice.toFixed(2)} zł</div>
                </div>

                <div class="cart__item-sum">
                    <div class="cart__item-sum-price js-cart-item-full-price">${(
                      (Math.round(book.FullPrice * 100) / 100) *
                      book.Quantity
                    ).toFixed(2)} zł</div>
                </div>
            </div>
            `;
    });

    const cartContainer = document.querySelector('.js-cart-container');
    cartContainer.innerHTML = template;

    setCartPrice();
    setCartPromotion();
  }

  const updateRecord = (book, quantity) => {
    $.ajax({
      type: 'PATCH',
      url: '/Cart/UpdateQuantity',
      data: { bookId: book, Quantity: quantity },
      dataType: 'json',
      success: function () {},
      error: function (err) {
        console.log(err);
      },
    });
  };

  const deleteRecord = (button, book) => {
    $.ajax({
      type: 'DELETE',
      url: '/Cart/DeleteItem',
      data: { bookId: book },
      dataType: 'json',
      success: function () {
        deleteButtonClicked = false;
        const navCartQuantityItem = document.querySelector('.js-nav-cart-quantity');
        navCartQuantityItem.innerText = navCartQuantityItem.innerText * 1 - 1;

        const cartItem = button.closest('.js-cart-item');
        cart.removeChild(cartItem);
        const cartItemsCountElement = document.querySelector('.js-cart-quantity');
        cartItemsCountElement.dataset.count -= 1;
        cartItemsCountElement.innerText = `${cartItemsCountElement.dataset.count} przedmioty`;
        setCartPrice();
        setCartPromotion();

        if (cartItemsCountElement.dataset.count == 0) {
          window.location.href = '/Cart/Empty';
        }
      },
      error: function (err) {
        deleteButtonClicked = false;
        console.log(err);
      },
    });
  };

  const updateNavigationCart = () => {
    const myStorage = window.localStorage;
    const items = JSON.parse(myStorage.getItem('cartItems')) || new Array();

    const cartQuantityElement = document.querySelector('.js-nav-cart-quantity');
    cartQuantityElement.innerText = items.length;
  };
})();
