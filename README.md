# emails-service

![Capture](https://user-images.githubusercontent.com/54571357/176462577-0c1d2524-8e2c-4c02-abaf-e1b32b872ef5.JPG)
messages that are sended using background service

![image](https://user-images.githubusercontent.com/54571357/176464233-c0ea6e2b-7a27-4843-999c-1906bae5ce64.png)
login dialog; email and password validation; user data is stored inside userService

![image](https://user-images.githubusercontent.com/54571357/176464773-7d8c7ca6-5f75-4452-ac3e-b4bdad7df4c5.png)
users list; available for admin only

![image](https://user-images.githubusercontent.com/54571357/176465061-17207794-1c69-4218-87de-7d1fd6e9a5ba.png)
task list; ability to delete and edit tasks implemented; the same component is used to edit and add task

![image](https://user-images.githubusercontent.com/54571357/176465420-5f334867-e11a-4b3f-ad1f-cb6ead91c6ab.png)
create/edit task component; angular component for cron is used (+ library to parce cron on backend)

about everthing else that was used on frontend: quard, routes, angular material, services, observable
for backend: rest api, three layers, automapper, hosted service, options pattern, cronos, mailkit

changes
![image](https://user-images.githubusercontent.com/54571357/177085809-140f812c-7f0b-4d89-8c49-965816ed9bb8.png)
users statictics was added

![image](https://user-images.githubusercontent.com/54571357/177085854-27e170f0-6469-4b3b-8206-2a5a2b9f7050.png)

![image](https://user-images.githubusercontent.com/54571357/177085893-a4f62690-3cb2-45da-b5d6-30aedeeeb79a.png)
topic settings were added

![image](https://user-images.githubusercontent.com/54571357/177085973-8ade3892-bea5-4e82-9cb4-f3a9b987378d.png)
moment to start send tasks was added

![image](https://user-images.githubusercontent.com/54571357/177086382-badf4f01-7e8e-4e92-96d7-dbd39e949f82.png)
csv files processing was added

multiple threads works with sending email tasks;
logout was added;
api structure changed, data is stored in db now;
role based token auth with expiration time

