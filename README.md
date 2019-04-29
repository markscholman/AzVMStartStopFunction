# AzVMStartStopFunction

A simple function that start or stops a VM in your subscription.
Specify in the application settings the Tenant Id and service principal SPN (client id and client secret)

Once deployed call the function url like:

```curl
Stop VM testvm001
curl 'http://localhost:7071/api/ControlVM?name=testvm001&action=stop&subscription=xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx0&resourcegroupname=vm-test-rg'

Start VM testvm001
curl 'http://localhost:7071/api/ControlVM?name=testvm001&action=start&subscription=xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx0&resourcegroupname=vm-test-rg'
```

This is useful for demo environments, not suitable for production! Set the authentication on function level and rotate them after each class/demo so you can reuse the Azure Function.
