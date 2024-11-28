(function () {
  const searchInput = document.querySelector('.js-search-navigation-input');
  const searchButton = document.querySelector('.js-search-button');

  searchButton.addEventListener('focus', e => {
    searchInput.focus();
  });
})();

