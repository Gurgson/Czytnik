(function () {
  const myStorage = window.localStorage;

  const isUserLogged = document.querySelector('.js-navigation-cart').dataset.logged;
  if (isUserLogged != 'True') {
    const items = JSON.parse(myStorage.getItem('cartItems'));
    if (items == null || items.length == 0) {
      window.location.href = '/Cart/Empty';
      return;
    }
  }
})();
