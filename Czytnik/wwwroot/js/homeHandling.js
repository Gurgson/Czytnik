(function () {
  const homeTopBooks = document.querySelector('.js-home-top-books');
  const carousel = document.querySelector('.js-books-container');
  if (carousel.dataset.count == 0) homeTopBooks.style.display = 'none';
})();
