(function () {
    const isUserLogged = document.querySelector('.js-navigation-cart').dataset.logged;

  const addCartItem = button => {
    const book = button.dataset.book;
    const price = button.dataset.price;
    const myStorage = window.localStorage;

    if(isUserLogged == "True")
        addRecord(book);
    else{
        const items = JSON.parse(myStorage.getItem('cartItems')) || new Array();
        if(items.some(item => item.bookId == book)) return;

        items.push({ bookId: book, Quantity: 1, price: price });
        myStorage.setItem('cartItems', JSON.stringify(items));
        updateNavigationCart();
    }

    button.classList.add('books-carousel__button--disabled');
    button.innerHTML = 'Dodano do koszyka';
    button.disabled = true;
  };

  const addSingleItem = button => {
    const book = button.dataset.book;
    const price = button.dataset.price;
    const myStorage = window.localStorage;

    const items = new Array();

    items.push({ bookId: book, Quantity: 1, price: price });

    myStorage.setItem('singleItem', JSON.stringify(items));
    myStorage.setItem('singleFull', price);
    myStorage.setItem('type', 'single');

    window.location.href = '/Checkout';
  };

  const booksContainer = document.querySelector('.js-books-container');

  booksContainer.addEventListener('click', e => {
    const addItemButton = e.target.closest('.js-cart-item-add');
    if (addItemButton) {
      e.preventDefault();
      addCartItem(addItemButton);
    }

    const addSingleButton = e.target.closest('.js-single-item-add');
    if (addSingleButton) {
      e.preventDefault();
      addSingleItem(addSingleButton);
    }
  });

  const addRecord = (book) => {
      $.ajax({
        type: 'POST',
        url: '/Cart/AddItem',
        data: { bookId: book },
        dataType: 'json',
        success: function () {
          const navCartQuantityItem = document.querySelector('.js-nav-cart-quantity');
          navCartQuantityItem.innerText = navCartQuantityItem.innerText * 1 + 1;
        },
        error: function (err) {
          console.log(err);
        },
      });
  }

  const updateNavigationCart = () => {
      const myStorage = window.localStorage;
      const items = JSON.parse(myStorage.getItem('cartItems')) || new Array();

      const cartQuantityElement = document.querySelector('.js-nav-cart-quantity');
      cartQuantityElement.innerText = items.length;
  }
})();