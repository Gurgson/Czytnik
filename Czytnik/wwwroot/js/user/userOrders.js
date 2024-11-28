const ordersContainer = document.getElementById("userOrders");
const take = 10;
let skip = 0;
let sortBy = "date_desc";

const moreButton = document.getElementById("moreButton");
const sortOptions = document.getElementById("sort");
const ordersCount = document.getElementById("ordersCount");

const handleChange = (e) => {
    sortBy = e.target.value;
    skip = 0;
    ordersContainer.innerHTML="";
    moreButton.disabled = false;
    loadOrders();
}

const showTwoD = (number) => {
    return (Math.round(number * 100) / 100).toFixed(2);
}

const getOrder = (order) => {
    let itemsMarkup = "";
    const items = order.Items.map(item => {
        itemsMarkup += `<li class="order__item">
            <p class="order__title" title="${item.BookTitle}">${item.BookTitle}</p>
            <span class="order__count">x${item.Quantity}</span>
            <span class="order__price">${showTwoD(item.Price)}zł</span>
        </li>`;
    });

    const markup = `
    <div class="order">
        <div class="order__info">
            <span class="order__date">${order.OrderDate.slice(0,10)}</span><span class="order__number">Zamówienie nr #${order.OrderId}</span>
        </div>
        <ul class="order__items">
            ${itemsMarkup}
        </ul>
        <div class="order__wholePrice">${showTwoD(order.CalculatedPrice)}zł</div>
    </div>
    `;

    return markup;
}

const loadOrders = () => {
    displaySpinner();
    $.ajax({
        type: "GET",
        url: "/User/GetAllOrders",
        data: {skip: skip, count: take, sortBy: sortBy},
        dataType: 'json',
        success: function (orders) {
            skip+=take;
            orders.forEach(o => {
                let content = getOrder(o);
                ordersContainer.innerHTML+=(content);
            });
            hideSpinner();
            if(orders.length ==0) {
                moreButton.removeEventListener('click',loadOrders);
                moreButton.disabled = true;
            }
        },
        error: function (emp) {
            alert('Coś poszło nie tak :(');
        }
    });
}

moreButton.addEventListener('click',loadOrders);
sortOptions.addEventListener('change',handleChange);
loadOrders();