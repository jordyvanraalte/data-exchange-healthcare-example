# Patient Data Exchange System Example
This is an .NET Core REST API built in C# that allows heatlhcare organisations to exchange patient data (in form of files) through an API. The API is designed with security in mind. 
The system includes validation checks for file uploads including checking file signatures and extensions. 
Moreover, the API contains error handeling and logging capabilities to keep track of all data exchanges between the parties.
To ensure the intergrity of the files, the hashes of the files are checked before passing them down to the other healthcare organisations.
The patient file is deleted after its download.

## Design
![design image](https://github.com/jordyvanraalte/data-exchange-healthcare-example/blob/main/images/design.png)
In the design there is an example between a nursing home and a hospital. The nursing home requests patient data from the hospital.
The hospital creates a file and sends it to the API. The API saves it through a PatientData and sends a "key" back to the hospital.
The hospital then sends the key to the nursing home. The nursing home can access the file through the key by requesting the file through the API.

The API has validion in form of signature detection and extension checking. Logging is done on upload and download by logging the ip address of the actor, the filename, file hash and timestamp of the request.
The API ensures the integrity of the file by checking the hashes. The file is deleted after download. 

## Reflection
Although I completed the project within the given four hours, I recognize that there are some areas for improvement in terms of the user experience. For example, the current implementation of the file download feature deletes the file after download, which might not be ideal for users who want to download the same file multiple times. To address this, I could consider introducing a remove timer that would allow the file to be removed only after a certain period of time has passed.

In addition, I realize that the current logging functionality is not very informative, as it only logs the IP address of the user when a file is uploaded or downloaded. While this was within the scope of the project, it would be beneficial to have more robust identification and logging capabilities in the future. This would allow for better tracking and analysis of file usage, which could be useful for auditing and compliance purposes.
