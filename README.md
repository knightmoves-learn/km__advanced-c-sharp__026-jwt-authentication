# 025 JWT Authentication

## Lecture

[![# Jwt Authentication (Part 1)](https://img.youtube.com/vi/98NJzlKtWew/0.jpg)](https://www.youtube.com/watch?v=98NJzlKtWew)
[![# Jwt Authentication (Part 2)](https://img.youtube.com/vi/9xNA228pPD8/0.jpg)](https://www.youtube.com/watch?v=9xNA228pPD8)

## Instructions

Prior to you starting this assignment the commands `dotnet remove package AspNetCore.Authentication.Basic` and `dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0` have been run, you do NOT need to run these yourself

You will also find your JWT Authentication configuration pre-filled for you to view in `HomeEnergyApi/secrets.json`

In `HomeEnergyApi/Controllers/AuthenticationController.cs`
- Create a public class `AuthenticationController` implementing `ControllerBase`
    - Create three private readonly properties `_issuer`, `_audience` and `_secret` of type `string`
    - Create a constructor taking one argument of type `IConfiguration`
        - In the body of the constructor, set `_issuer` `_audience` and `_secret` to the issuer, audience and secret from `secrets.json`
    - Create a `Token()` method, returning an `IActionResult` and designate it as a HTTP POST route at `/Authentication/token` with the attribute `[HttpPost("token")]`
        - `Token()` should set a `string` variable to the result of `GenerateJwtToken()` (which we will define later)
        - `Token()` should return `Ok()` with the result of `GenerateJwtToken()` supplied as an argument inside of an anonymous object
    - Create a `GenerateJwt()` method with a return type of `string`
        - `GenerateJwt()` should create a variable holding a new `SymmetricSecurityKey` with the `byte[]` value of `_secret` passed into its constructor
        - `GenerateJwt()` should create a variable holding a new `SigningCredentials` with the newly created `SymmentricSecurityKey` and `SecurityAlgorithms.HmacSha256` passed into its constructor 
        - `GenerateJwt()` should create a new array holding two `Claim` objects
            - The first `Claim` should have `JwtRegisteredClaimNames.Sub` and `"maria@knightmove.com"` passed into its constructor
            - The second `Claim` should have `JwtRegisteredClaimNames.Jti` and a new `Guid` as type `string` passed into its constructor
        - `GenerateJwt()` should create a variable holding a new `JwtSecurityToken` witth the following passed into its constructor...
            - `_issuer` as "issuer"
            - `_audience` as "audience"
            - The the variable holding the `Claim` array you created as "claims"
            - An hour from the current time as "expires" (An hour from when the code runs, not the time you are writing this)
            - The variable holding the `SigningCrendentials` you created as "signingCredentials"
        - `GenerateJwt()` should return `new JwtSecurityTokenHandler().WriteToken()` with the variable holding the `JwtSecurityToken` you created passed into `WriteToken()`

In `HomeEnergyApi/Authorization/JwtAuthenticationHandler.cs` (Note: This file has been renamed for you)
- Rename the class `BasicAuthenticationHandler` to `JwtAuthenticationHandler`
- Remove the existing properties and create three private readonly properties `_issuer`, `_audience` and `_secret` of type `string`
- Replace the body of the constructor with code that sets `_issuer`, `_audience` and `_secret` to the value of Jwt.Issuer, Jwt.Audience, and Jwt.Secret in `secrets.json`
- Replace the body of the try block...
    - Create a variable holding the authorization token value by accessing `Request.Headers["Authorization"]` as a `string`, calling `String.Split()` with `" "` (empty space) passed, and accessing the last element of the resulting array
    - Create a variable holding a new `JwtSecurityTokenHandler`
    - Create a variable holding the `byte []` value of `_secret`
    - Create a variable holding a new `TokenValidationParmeters` with...
        - `true` as "ValidateIssuer", "ValidateAudience" and "ValidateIssuerSigningKey"
        - `_issuer` as "ValidIssuer`
        - `_audience` as "ValidAudience`
        - A new `SymmetricSecurityKey` with the variable holding the `byte []` value of `_secret` passed into its constructor
    - Create a variable holding the result of `tokenHandler.ValidateToken()` with the variable holding the authorization token, the variable holding the `TokenValidationParameters`, and `out var validatedToken`
    - Create a variable holding a new `AuthenticationTicket` with the result of `tokenHandler.ValidateToken()` and `Scheme.Name` passed into its constructor
    - Return `AuthenticateResult.Success(ticket)` with your `AuthenticationTicket` passed into `.Success()`

In `HomeEnergyApi/Program.cs`
- Replace the refrence to `BasicAuthenticationHandler` with `JwtAuthenticationHandler`
- Replace any instance of `"BasicAuthenticationHandler"` with `JwtAuthenticationHandler`

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
