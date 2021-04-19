# API.Customer

### Introduction
API.Customer is an api designed for storing, modifying and deleting customer information.

### Project structure
To build this project I've used a simple 3-layer structure. It also uses dependency injection (DI) according to the SOLID principles. <br />
##### API.Customer.Web
This is the presentation layer. It contains the apis controller endpoint and registration of DI components
##### API.Customer.Business
This is the business layer. It contains all the business logic for the api. <br /> 
Here you can find logic for formatting and validating customer input aswell as what and when calls to external systems should be made.
##### API.Customer.Data
This is the data layer. It contains all dependencys to external systems including the database. <br />
<b>HRSystem:</b> This system does not currently exist but are the external api that are meant for validating customers official id (perssonnummer). 
It is registred with a mocked HttpClient that never makes the actuall call but always say that the validation was successfull. To use real validation here just comment out this code:

```cs
      //services.AddHttpClient(); //This row should be used to use a real IHTTPClientFactory
      services.AddTransient<IHttpClientFactory, DummyHttpClientFactoy>();  //Mocked client since this system dosn't exist at the moment
```
<b>Customer database:</b> This is an external database hosted in Azure. All calls to the database is being made with sql-injection protected code in DatabaseProvider.cs

### Usage
The api is currently hosted as an docker image hosted freely by appfleet. It has a swagger for easy access that can be found here: <br />
<a>http://stefftest-690c73.appfleet.net:8080/swagger/index.html</a> <br />
To start testing you may start with using the Post call and press "Try it out". There is validation on the call as specified in the requirements of the task.
Example of a working call to test:
```json
{
  "officialId": "19910101",
  "email": "test@test.se",
  "adress": {
    "zipCode": "110 10",
    "country": "sweden"
  },
  "phoneNumber": "012312312"
}
```

### Building and updating the api in the docker image
The application can be opened in visual studio and built there. Once completed you may use the built in publish functionality of Visual Studio and publish the container to docker hub.
Once in docker hub import it into your desired host service and start it. The internal ports need to be set to 80 and 443.


### Things I would have improved if I had more time to work on it
There is a big lack of unit tests of the system as it is currently. It was not something I prioritized with the limited time I had since this system will never go to production anyway.
All classes is designed by the SOLID principals thought which makes unit tests very easy to make and the whole system has been designed with unit tests and DI in mind. 
<br /> <br />
Another thing that may be improved is validation. I quickly added the validation specified in the requirements and some others that seemed important but I still havn't put much more thought into it.
there may very well be more that would be nice to add.
<br /> <br />
Error responses from the api are not always very clear as of what went wrong. This is something that by Skandias standard is often vague as to security reasons and more info are often provided in logs.
Since there is no external source of logging there can sometimes be hard to know what went wrong. A serilog sink should be used to somewhere a developer has read access.
<br /> <br />
The swagger has build in dokumentation that is taken from code comments. The swagger could have been made clearer and more easy to use if I spent more time dokumenting things in-code. Unfortunately I ran out of time
