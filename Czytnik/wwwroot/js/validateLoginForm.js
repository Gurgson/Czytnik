const form = document.getElementById("form");
const emailInput = document.getElementById("emailInput");
const passwordInput = document.getElementById("passwordInput");

const formTruthTable = {
    email: false,
    password: false,
};

emailInput.addEventListener('change',e=>{
    formTruthTable.email = validateEmail(e.target);
    if(!formTruthTable.email){
        e.target.parentNode.classList.add('form__label--error');
    } else {
        e.target.parentNode.classList.remove('form__label--error');
    }
});
passwordInput.addEventListener('change',e=>{
    formTruthTable.password = validatePassword(e.target);
    if(!formTruthTable.password){
        e.target.parentNode.classList.add('form__label--error');
    } else {
        e.target.parentNode.classList.remove('form__label--error');
    }
});

form.addEventListener('submit',e=>{
    e.preventDefault();
    let allow = true;
    for (const [key, value] of Object.entries(formTruthTable)) {
        allow *= value;
    }
    if(allow)form.submit();
});