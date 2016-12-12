(function () {
    "use strict";

    
    



    angular.module("app-auth")
    .controller("authenticationController", authenticationController);

    function authenticationController($http,$scope,$timeout)
    {
        //object for registration validation
        
        //user registration object
        $scope.UserInfo = {
            Username : "",
            Email : "",
            ConfPassword : "",
            Password : ""
        }
        $scope.showLog = true;
        $scope.showReg = false;
        
        
        $scope.switchLogin = function () {
            $scope.showLog = true;
            $scope.showReg = false;
        }
        $scope.switchReg = function () {
            $scope.showLog = false;
            $scope.showReg = true;
        }
       



        $scope.registerUser = function () {

            if (ValidateReg($scope.UserInfo)) {

                var rvm = {
                    Username: $scope.UserInfo.Username,
                    Password: $scope.UserInfo.Password,
                    Email: $scope.UserInfo.Email
                }
                $http.post("/register", rvm)
                .then(function (response) {
                    if (response.status == 201) {
                        $scope.message = "User created succesfully. You need to login"
                        $scope.UserInfo = {}

                    } else {
                        $scope.UserInfo.Password = {}
                        $scope.UserInfo.ConfPassword = {}
                        $scope.message = response.data;
                    }
                })
                .finally(function () {
                });
            }
        };



        // check if user input is correct
         var  ValidateReg = function (userInfo)
         {
             $scope.regValid = {
                 Username: { valid: true, errorMessage: "" },
                 Email: { valid: true, errorMessage: "" },
                 Password: {
                     valid: true, errorMessage: {
                         lengthError: "",
                         alphaError: "",
                         digitError: "",
                         upperError: ""
                     }
                 },
                 ConfPassword: { valid: true, errorMessage: "" }
             }
             var valid = true;
             if (userInfo.Username.length < 3 || userInfo.Username.length > 32)
             {
                 $scope.regValid.Username.valid = false;
                 $scope.regValid.Username.errorMessage = "Username must be between 3 and 32 characters long";
                 valid = false;
             }
             if (!validateEmail(userInfo.Email)) {
                 $scope.regValid.Email.valid = false;
                 $scope.regValid.Email.errorMessage = "Email address is not valid";
                 valid = false;

             }
             //========================================= wrap in function BEGIN
             if (!validatePassword(userInfo.Password,1)) {
                 $scope.regValid.Password.valid = false;
                 $scope.regValid.Password.errorMessage.lengthError = "Must contains at least 8 characters";
                 valid = false;
             }
             if (!validatePassword(userInfo.Password, 2)) {
                 $scope.regValid.Password.valid = false;
                 $scope.regValid.Password.errorMessage.alphaError = "Must have at least one non alphanumeric character";
                 valid = false;
             }
             if (!validatePassword(userInfo.Password, 3)) {
                 $scope.regValid.Password.valid = false;
                 $scope.regValid.Password.errorMessage.digitError = "Must have at least one digit ('0'-'9')";
                 valid = false;
             }
             if (!validatePassword(userInfo.Password, 4)) {
                 $scope.regValid.Password.valid = false;
                 $scope.regValid.Password.errorMessage.upperError = "Must have at least one uppercase ('A'-'Z')";
                 valid = false;
             }
             //========================================= wrap in function END
             if (userInfo.Password != userInfo.ConfPassword)
             {
                 $scope.regValid.ConfPassword.valid = false;
                 $scope.regValid.ConfPassword.errorMessage = "Passwords do not match"
                 valid = false;
             }

             
            return valid;
        }
        

    }
    function validatePassword(password,alt)
    {
        switch(alt){
        case 1:
        {
            var re = /[A-Za-z\d$@#$^!%*?&]{8,}/;
            return re.test(password);
        }
        case 2:
        {
            var re = /^(?=.*[$@#$!%^*?&])[A-Za-z\d$@#$^!%*?&]/;
            return re.test(password);
        }
       case 3:
        {
            var re = /^(?=.*\d)[A-Za-z\d$@#$^!%*?&]/;
            return re.test(password);
        }
        case 4:
        {
            var re = /^(?=.*[A-Z])[A-Za-z\d$@#$^!%*?&]/;
            return re.test(password);
        }
    }
    }

    
    function validateEmail(email) {
        var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{1,}))$/;
        return re.test(email);
    }

})();


// Damusti password checka. 