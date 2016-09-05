# git workflow

* pull from master
* make a new feature branch
* make a new scene/reuse older test scene (only to be used by you)
* do all work in that scene
* prefab all objects made
* (optional) export prefabs as unity assets, store in a different folder (gdrive?)
* commit changes, with descriptive comments
* push and merge!

# file naming

* Always capitalize in PascalCase, e.g. FileName.cs

# variables

* Always camelCase
* always use setters and getters in game managers/scripts that will be accessed by others
* always use local references, rather than directly referencing a variable on another script
	* for example, if you're accessing variableName in the GameManager, do variableName = GameManager.variableName;, and only refence that local var
