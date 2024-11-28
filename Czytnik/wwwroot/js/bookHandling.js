(function () {
  const navigationCart = document.querySelector('.js-navigation-cart');
  const productFavourite = document.querySelector('.js-product-favourite');

  if (navigationCart.dataset.logged == 'False') productFavourite.style.display = 'none';
})();
