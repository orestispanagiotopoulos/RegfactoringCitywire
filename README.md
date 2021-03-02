# RegfactoringCitywire

I have made the decision to move the business logic from the CustomerService to the Customer entity. 
The customerService shouldn’t really know how to apply the credit limit to the Customer. 
I believe this logic belongs to the Customer entity.

I have moved the validation logic inside the Customer constructor to prevent the creation of 
a Customer with invalid data. Another option would be to have a dedicated validation class 
to validate the input parameters.

Because of the time restrictions I have refactored and tested mainly the classes CustomerService and 
Customer.  Next step would be to refactor the repositories.

I have used visual studio 2019. All the tests are passing.

