Feature: Google Search 
	In order to to find new information
	I will google the following terms
	Because I want to be a genius

@FunctionalUI
Scenario Outline: Search Google
	Given I am using the follwing '<Device>'
	And I have searched for '<SearchTerm>'
	Then search results should be returned

	Examples: 
	| Device                     | SearchTerm                                     |
	| IPhone4SLandscape          | Barney Loves me                                |
	| IPhone4SPortrait           | Hello World!!                                  |
	| IPhone5SLandscape          | This is text                                   |
	| IPhone5SPortrait           | This is a demo                                 |
	| IPadLandscape              | These searches are logged to my google account |
	| IPadPortrait               | The world is ending                            |
	| SamsungS3MiniLandscape     | Hello                                          |
	| SamsungS3MiniPortrait      | Bye                                            |
	| SamsungS4Landscape         | Software Developer                             |
	| SamsungS4Portrait          | Windows 98                                     |
	| NokiaLumia610Landscape     | Open Box Software                              |
	| NokiaLumia610Portrait      | StackoverFlow                                  |
	| WindowsSurfaceProLandscape | I am silly                                     |
	| WindowsSurfaceProPortrait  | Do we really need to do this                   |
	| MotorolaXoomLandscape      | There is meaning to life                       |
	| MotorolaXoomPortrait       | There is no meaning to cats on the internet    |