const filterForm = document.querySelector(".js-filter-form");
const searchForm = document.querySelector(".js-search-form");


filterForm.addEventListener('submit', (e) => {
    e.preventDefault();

    const searchNavigationInput = document.querySelector(".js-search-navigation-input");
    const searchFormInput = document.querySelector(".js-search-form-input");

    searchFormInput.value = searchNavigationInput.value;

    e.target.submit();
})

searchForm.addEventListener('submit', (e) => {
    e.preventDefault();

    const searchNavigationInput = document.querySelector(".js-search-navigation-input");
    const searchFormInput = document.querySelector(".js-search-form-input");

    searchFormInput.value = searchNavigationInput.value;

    filterForm.submit();
})