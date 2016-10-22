# ErrorUnit.Logger_Elmah
For ErrorUnit to work with your Elmah; add ErrorUnitCentral._Logger = new ErrorUnitLogger(); where your application start code is.
Also remember to add `applicationName` to your Elmah config `<errorLog type=` ... ` applicationName="WebApplication9"/>`; or ErrorUnit will not be able to retrieve errors.
http://johngoldinc.com/Help/html/T_ErrorUnit_Interfaces_ILogger.htm
