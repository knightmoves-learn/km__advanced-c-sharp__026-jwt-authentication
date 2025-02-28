# 025 Basic Authentication

## Lecture

[![# Basic Authentication (Part 2)](https://img.youtube.com/vi/h8DWSgf2PfM/0.jpg)](https://www.youtube.com/watch?v=h8DWSgf2PfM)
[![# Basic Authentication (Part 1)](https://img.youtube.com/vi/h8DWSgf2PfM/0.jpg)](https://www.youtube.com/watch?v=h8DWSgf2PfM)

## Instructions

In `HomeEnergyApi/secrets.json`
- Add a `BasicAuth` property
    - The value should be an object with a `Username` property with a value of `username` and a `Password` property with a value of `password`

In order to autograde your code challenge, you will NOT add your `secrets.json` file to the `.gitignore`. This is something you would want to do at this step otherwise.

In `HomeEnergyApi/Program.cs`
- Call `builder.Configuration.AddJsonFile()` and pass to it the argument `secrets.json`

In `HomeEnergyApi/Authorization/BasicAuthenticationHandler.cs`
- Create a new public class `BasicAuthenticationHandler` implementing `AuthenticationHandler<AuthenticationSchemeOptions>`
    - Create two private readonly properties `_username` and `_password` of type `string`
    - Create a constructor with an argument for each of the following types...
        - `IOptionsMonitor<AuthenticationSchemeOptions>`
        - `ILoggerFactory`
        - `UrlEncoder`
        - `ISystemClock`
        - `IConfiguration`
    - Add `: base(options, logger, encoder, clock)` after the constructor's argument list to designate the parent class constructor to be used
    - In the body of the constructor, set `_username` and `_password` to the `Username` and `Password` from `secrets.json` using the `IConfiguration` argument
    - Create a protected override async method `HandleAuthenticateAsync()`
        - `HandleAuthenticateAsync()` should return a `Task<AuthenticateResult>`
        - `HandleAuthenticateAsync()` should return `AuthenticateResult.Fail("Missing Authorization header")` if the request headers do NOT contain an `Authorization` key
        - Within a try block..
            - Set a variable to the authorization header using `AuthenticationHeaderValue.Parse(Request.Headers["Authorization"])`
            - Set a variable to the byte value of the authorization header by using `Convert.FromBase64String` on it's `Paramter` property
            - Set a variable to an array containing both authorization string values by converting the byte value of the authorization header with `Encoding.UTF8.GetString()` and using `String.Split()` to split that value on the `:` character
            - Set a variable to the first value in the resulting array to get the username, and the second value to get the password
            - If the username and password variables you retrieved match the `_username` and `_password` properties...
                - Create a variable holding an array holding a new `Claim` passing `ClaimTypes.Name` and the retrieved username into its constructor
                - Create a variable holding a new `ClaimsIdentity` passing your new claims array and `Scheme.Name` into its constructor
                - Create a variable holding a new `ClaimsPrinciple` passing the newly created `ClaimsIdentity` into its constructor
                - Create a variable holding a new `AuthenticationTicket` passing the newly created `ClaimsPrinciple` and `Scheme.Name` into its constructor
                - Return `AuthenticateResult.Success()` with your `AuthenticationTicket` passed into `.Success()`
            - If the username and password variables you retrieved do NOT match the `_username` and `_password` properties, return `AuthenticateResult.Fail("Invalid username or password")`
        - Within your catch block return `AuthenticateResult.Fail("Invalid username or password")`

In `HomeEnergyApi/Program.cs`
- Call `builder.Services.AddAuthentication("BasicAuthentication")`
    - On this, call `.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null)`
- Call `builder.Services.AddAuthorization()`
- Call `UseAuthentication()` and `UseAuthorization` on the `app`

In `HomeEnergyApi/Controllers/HomeAdminController.cs`
- Add the `[Authorize]` tag to the `CreatHome()` method

## Additional Information
- Do not remove or modify anything in `HomeEnergyApi.Tests`
- Some Models may have changed for this lesson from the last, as always all code in the lesson repository is available to view
- Along with `using` statements being added, any packages needed for the assignment have been pre-installed for you, however in the future you may need to add these yourself

## Building toward CSTA Standards:
- Give examples to illustrate how sensitive data can be affected by malware and other attacks (3A-NI-05) https://www.csteachers.org/page/standards
- Recommend security measures to address various scenarios based on factors such as efficiency, feasibility, and ethical impacts (3A-NI-06) https://www.csteachers.org/page/standards
- Compare various security measures, considering tradeoffs between the usability and security of a computing system (3A-NI-07) https://www.csteachers.org/page/standards
- Explain tradeoffs when selecting and implementing cybersecurity recommendations (3A-NI-08) https://www.csteachers.org/page/standards
- Compare ways software developers protect devices and information from unauthorized access (3B-NI-04) https://www.csteachers.org/page/standards
- Explain security issues that might lead to compromised computer programs (3B-AP-18) https://www.csteachers.org/page/standards

## Resources
- https://learn.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-9.0

Copyright &copy; 2025 Knight Moves. All Rights Reserved.
