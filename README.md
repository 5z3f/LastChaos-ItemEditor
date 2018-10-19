# L-Chaos Item Editor
![Version](https://img.shields.io/badge/version-1.0-blue.svg?style=for-the-badge)
![Platform](https://img.shields.io/badge/platform-windows-green.svg?style=for-the-badge)
![license](https://img.shields.io/badge/license-GPL%203.0-orange.svg?style=for-the-badge)

![image1](hhttps://raw.githubusercontent.com/IhsotasOtomakan/LastChaos-ItemEditor/master/preview.png)

One of my old projects which I sadly did not finish, sharing for educational purposes, it's not complete tool.\
It was mostly based on **Wizatek's ItemAll Editor**, so credits goes to him too.\
**[@Karmel0x](https://github.com/Karmel0x/)** was helping me in that time with it too so I guess I should credit him aswell.

## Features:
- Optimized L-Chaos Texture reading class, added ```old file (Version 4)``` and ```DXT5``` support, added texture flags viewer
- Optimized L-Chaos Mesh reading class
- Added async support to MySQL class
- [Icon Picker](http://grabilla.com/0881e-785ca3b8-6f2c-4119-89cb-73d080b36ad1.png)
- [Model Viewer Tab](http://grabilla.com/0881e-8a224ed4-2c29-485c-bf87-d8f7a867619b.gif) - Item equiped class and standalone model (needs to be HEAVILY optimized, its straight copy pase)
- [Effect Helper Form](http://i.grab.la/0881e-ffe6de6e-f71d-4b38-8e88-aaac81b26393.png) - shows which ```Item``` ```Skill``` ```Monster``` ```Title``` is using an effect name
- [Item Drop Data Tab](http://grabilla.com/0881e-e1405316-7141-4508-9b7d-264e4234ffd4.png) - shows which monster drops current itemid with their position on the map (dungeons not supported, map writing function was writed for testing purposes so I would look into optimizing this one too)
- [RareOption Picker](http://grabilla.com/0881e-1c20630b-f2b2-47d4-a6a0-b84960b39f89.png)
- [Database Connection Settings](http://grabilla.com/0881e-2b3210e7-c5fa-4bf0-9f9e-a6d930b9e1cf.png) are no longer in binary mode, changed to JSON
- [SMC Viewer/Editor](http://grabilla.com/0881e-b4f0eee1-c983-4be8-b111-f8cb8787feeb.png)
- Added sorting types - Name, Description, [Type](http://grabilla.com/0881e-47f130c6-9d98-4ff6-866c-36313610e6c8.png), [JobFlag](http://grabilla.com/0881e-89afbf80-63fb-4044-837b-d3065c763bf1.png), [Flag](http://grabilla.com/0881e-b926df11-73c6-4486-a370-79a8831fdc6d.png)
- Flag is no longer bugging (most of item tools I've saw was reading it as uint32 instead of 64)
- All Flags/Types are listed in ENUM

## Missing:
- Whole Crafting Data, Static Bloodseals and Fortune Data tab but it can be easily implemented thought
- Logs are not implemented
- ZoneFlag picker is not implemented
- Effect picker is not implemented
- Export to L-Chaos item lod file is not implemented
- ModelInformation form is not completed, the idea was to get all textures from actual displaying model in DX panel and show its data/flags and rest data here
- Effect.dat reader, I've already started writing it on cEffect class

## Download
  - [Here](http://grabilla.com/0881e-6c9357f3-d8d4-48a2-b466-8e7dac655e9a.zip) is compiled version

## Donate
- If you want to support me throught cryptocurrency
  - BTC: 1EPcazBzV2qfyfUuRLoj9qeXmaPGnsKiF6
  - XMR: 42NRbCTQfZc7jMgJMwoKnQg26j8mzbHBZPecWJu4zpmJ4t5mHHWdLLa8qBqWhJ3BBUC2VLZjX39ENafMss1TnvDvNsQVUPM
