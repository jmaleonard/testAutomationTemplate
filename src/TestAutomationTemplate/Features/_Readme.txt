All your custom defined features are added here. 

Once your feature is defined. Right click on the feature and select "Generate Step Definitions"
A popup will appear and select copy methods to clipboard.

Once these methods are on your clipboard. Create a class in the Steps folder labelled <YourFeatureName>Steps.cs. And paste the steps into the newly created class.
Remember to add SpecFlow to the class and add [Binding] to the top of the class so that SpecFlow is albe to map the steps to this class. 
