# project-template-unity

[This](https://github.com/Chillu1/project-template-unity) is still a WIP project, after making my 3rd/4th proper project, I decided to make a bare bones skeletons for all my future projects, so new functionality will be added here with time.  
A project template with boilerplate code &amp; assets made specifically not to use precious time on miscellaneous stuff, aka not reinventing the wheel &amp; basic fundamental mechanics every project.

## Table of Contents

1. [Pre Conditions](#pre-conditions)
2. [Usage](#usage)
3. [FAQ](#faq)
4. [All imported resources](#all-imported-resources)

## Pre Conditions

Everything in this project is currently licensed under MIT, so use it however you want.  
This template will usually be tested one of the latest Unity LTS versions, so at least 2019.4 is advised.  
But overall, all 2017+ versions should work for the most part (apart from some UI dependencies &amp; changes)

## Usage

First way:

1. "Use this template"
2. Make your own repo
3. Git pull

Second way:

1. Clone this repo/download zip
2. Git pull/extract zip to specified folder
3. Open the project in Unity

Rename the namespace

## FAQ

Q: Why do you use "mainMenuButton.onClick.AddListener(delegate { SceneManager.LoadScene("mainmenu"); });" shinangas instead of doing it through the editor (like a normal human being)?  
A: When you work in a team, you usually don't want to fix merge conflicts with Unity scenes, so it's just safer to do most things through code. + It has some advantages. Like not loosing what corresponds to where in the scene when meta files get removed, or other troublesome issues.

Q: Why don't you use the Unity feature X, but instead make your own through code?  
A: It's impossible to know how Unity works behind the scenes, ex: gameobjects have huge overheads if not properly used/optimized. So coding the specific systems yourself gives you more freedom & control over everything.

Q: Why do you use GameController.cs as the main initiator for all non-monobehaviour scripts?  
A: I'm actually not sure about this one. It lets us have everything be centrilized, and have easier control over proper script execution, but if one thing fails, everything might. I'm up for suggestions here tbh.

Q: How do I contribute and/or report bugs?/I have an idea.  
A: [CONTRIBUTE.md](/CONTRIBUTE.md)

Q: Why isn't X or Y basic mechanic implemented?  
A: Not every mechanic is being used by most games, that's why there's no Camera control, 2d/3d movement, because they vary a lot from game to game.

## All imported resources

Currently uses modified terminal code from [Stillwwater's command terminal](https://github.com/stillwwater/command_terminal) (MIT)

[Singleton script](https://github.com/kleber-swf/Unity-Singleton-MonoBehaviour) (MIT)

Any questions that are not suited for "issues"? PM Chillu#8847 on Discord
