These steps are defined from within the Features. We Generate all the step definitions to the clipboard.
Then once done we add these to the the Steps folder.

-------------------------------------------------------------------------------------------------------------

BeforeAndAfter.cs
This class is the controller class for ALL specflow tests. 

This class contains the following attributes [BeforeTestRun] and [AfterTestRun]. 
These methods need to be static (Specflow standard.). If not test will not run.

Within the CommonSetup() you would usually add the following.
Creating the directory where all the temp files will be dropped. This MUST be the same as the what you configure Jenkins to archive.
If videorecording is set to True within the app.config we will kick off the viedo recording

Within the CommonTearDown() you would usually add the following.
Once all test are run this method would start running.

In here we stop the video recording. 
And generate the HTML report which will be added to temp Directory