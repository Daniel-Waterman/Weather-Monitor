# Weather Monitor

### [Go to the live website](https://agreeable-water-00cb02803.1.azurestaticapps.net) 

This is a web app that displays the latest data from my home-made weather station. My weather station uploads data to the thingspeak
IOT service which this app access through an API to download the data.

Please note that this is a very work-in-progress project and I am continually developing it.

[Click here](https://github.com/Dan-BW/Weather-Station) to see the code that is running the weather station itself.

### Details
The web app is built using Blazor and is hosted as a static web app using Microsoft Azure. It consists of the Client, which contains
the pages and logic required to load the web page; the API which contains the server side logic and connects with the
client via a web API; and Shared, which contains models which are shared betweent the client and server logic.

While this is a static web page, with no permanent server connection, Microsoft Azure allows me to have a backend API which securely
connects to Thingspeak to download the data.


### Future Developments
- Establish a real-time connection with server to update data as it arrives
- Create more detailed and modern dashboard (in a tab-style format rather than tabular)
- Create historical record tables for different time periods (e.g. day/week/month etc.)
