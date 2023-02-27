# Patient Data Exchange System Example
This is a .NET Core REST API built in C# that enables healthcare organizations to exchange patient data via an API. The system includes advanced validation checks for uploaded files, such as checking file signatures and extensions. In addition, the API features comprehensive error handling and logging capabilities to facilitate tracking of all data exchanges between the involved parties. The files are saved on disk and objects are stored in the SQLite database.

To ensure the security and integrity of the files, the API performs a hash check on the files before they are sent to other healthcare organizations. This guarantees that the data being shared is accurate and has not been tampered with. For further security, the patient file is automatically deleted from the server after being downloaded, protecting the patient's sensitive data.

## Design
![design image](https://github.com/jordyvanraalte/data-exchange-healthcare-example/blob/main/images/design.png)
The design of the system allows nursing homes to request patient data from hospitals through an API. When a nursing home makes a request, the hospital creates a file containing the requested data and sends it to the API. The API saves the file as a PatientData object and generates a unique "key", which is sent back to the hospital. The hospital then shares the key with the nursing home, which can use it to access the file through the API.

To ensure the security and integrity of the system, the API implements several validation measures, including signature detection and extension checking. When a file is uploaded or downloaded, the API logs the IP address of the actor, the filename, file hash, and timestamp of the request. This allows for easy tracking and auditing of file usage. Additionally, the API checks the hashes of uploaded files to ensure their integrity. Finally, to protect patient privacy, the system deletes files from the server after they have been downloaded by the nursing home

## Getting started
To run the application, firstly restore the Nuget packages:
```
dotnet restore
```
Secondly, create / update the SQLite database
```
dotnet ef database update
```
Lastly, run the application
```
dotnet run
```

## Reflection
Although I completed the project within the given four hours, I recognize that there are some areas for improvement in terms of the user experience. For example, the current implementation of the file download feature deletes the file after download, which might not be ideal for users who want to download the same file multiple times. To address this, I could consider introducing a remove timer that would allow the file to be removed only after a certain period of time has passed.

In addition, I realize that the current logging functionality is not very informative, as it only logs the IP address of the user when a file is uploaded or downloaded. While this was within the scope of the project, it would be beneficial to have more robust identification and logging capabilities in the future. This would allow for better tracking and analysis of file usage, which could be useful for auditing and compliance purposes.
