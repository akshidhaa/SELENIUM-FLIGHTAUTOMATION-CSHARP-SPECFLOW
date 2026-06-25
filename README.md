# SELENIUM-FLIGHTAUTOMATION-CSHARP-SPECFLOW
Flight Booking Automation using Selenium + SpecFlow (BDD) 



#FlightBDD_fixed/
└── FlightBDD_fix/                      MAIN PROJECT FOLDER
    │
    ├── FlightBDD.csproj                REQUIRED (core project file)
    ├── FlightBDD.csproj.bak
    ├── specflow.json                    SpecFlow config
    ├── README.md
    │
    ├── Features/                       BDD Feature files
    │   ├── FlightBooking.feature
    │   └── FlightBooking.feature.cs
    │
    ├── StepDefinitions/                 Step implementations
    │   └── FlightSteps.cs
    │
    ├── Pages/                           Page Object Model
    │   ├── BasePage.cs
    │   ├── ConfirmationPage.cs
    │   ├── FlightResultsPage.cs
    │   ├── FlightSearchPage.cs
    │   ├── PaymentPage.cs
    │   └── SeatBookingPage.cs
    │
    ├── Hooks/                           Setup / Teardown
    │   └── TestHooks.cs
    │
    ├── TestData/                        Test data
    │   └── users.json
    │
    ├── bin/                             AUTO-GENERATED
    │   └── Debug/
    │       ├── net8.0/
    │       └── net10.0/
    │
    └── obj/                             AUTO-GENERATED
        ├── Debug/
        │   ├── net8.0/
        │   └── net10.0/
        ├── project.assets.json
        ├── project.nuget.cache
        └── FlightBDD.csproj.nuget.*

Project Overview

This project focuses on automating the flight booking workflow using Selenium WebDriver integrated with SpecFlow (BDD framework) in a .NET (C#) environment.

The automation is implemented on the demo website:
https://phptravels.net

The goal of the project is to validate the end-to-end user journey starting from flight search to booking, payment, and ticket generation at the UI level.

Important Limitation

The selected application (phptravels.net) is a demonstration-based user interface and does not support real-time backend operations. Specifically:

- It does not support real-time API integration
- It does not provide actual flight inventory data
- It does not include a functional booking system
- It does not process real payment transactions

As a result, all search scenarios return "No Flights Available", regardless of input combinations.

Project Approach

Due to the absence of backend support, the automation strategy focuses on:

- Validating user interface behavior
- Handling "No Flights Available" scenarios
- Simulating booking flow steps such as:
  - Flight selection
  - Passenger details entry
  - Payment processing
  - Ticket generation

This ensures that the logical flow of the application is covered even without real data.


Automation Flow

The following workflow is implemented:

1. Launch flight booking website
2. Enter departure city
3. Enter destination city
4. Select departure date
5. Click search
6. Evaluate search results

   - If flights are available → proceed to next steps (rare scenario)
   - If no flights are available → handle gracefully and trigger simulation

7. Simulated flow includes:
   - Flight selection
   - Passenger detail entry
   - Payment completion
   - Ticket generation

Technologies Used

- Selenium WebDriver
- SpecFlow (BDD Framework)
- C# (.NET)
- NUnit
- ChromeDriver
- Visual Studio Code


xecution Summary:

- Total Test Cases: 4  
- Passed: 4  
- Failed: 0  
- Execution Time: ~110 seconds  

Each test case uses different combinations of:
- Departure city  
- Destination city  
- Departure date  

All scenarios are executed successfully with proper handling of no-flight conditions.
