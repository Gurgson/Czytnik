const validateEmail = (input) => {
    const validRegex =  /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
    return(input.value.match(validRegex))?true:false;
}

const validatePassword = (input) => {
    const validRegex =  /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$/;
    return(input.value.match(validRegex))?true:false;
}

const validateSecondPassword = (firstPassword, secondPassword) => {
    return(firstPassword.value===secondPassword)?true:false;
}

const validateUsername = (input) => {
    const validRegex =  /^[a-zA-Z0-9].{5,}$/;
    return (input.value.match(validRegex))?true:false;
}

const validatePhoneNumber = (input) => {
    const validRegex =  /^[0-9\+]{9,13}$/;
    return (input.value.match(validRegex) || input.value.length===0)?true:false;
}

const validateString = (string) => {
    return(string.trim().length>0);
}

const validatePictureInput = (filename) => {
    const extensions = ["jpg","png","jpeg","gif"];
    const fileExtension = filename.split('.').pop().toLowerCase();
    return (extensions.findIndex(el=>el===fileExtension) > -1);
}