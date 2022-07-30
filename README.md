<!-- Copyright (c) 2021-2022 Ishan Pranav. All rights reserved. -->
<!-- Licensed under the MIT License. -->

# Ishan Pranav\'s REBUS
A student-developed multiplayer space trading game. Please see the details of the design [here](Design.md).
## Documentation
The public API is documented [here](https://ishanpranav.github.io/rebus/pages/Rebus.html).
## License
This repository is licensed with the [MIT](LICENSE.txt) license.
## Attribution
This software uses third-party libraries or other resources that may be
distributed under licenses different than the software. Please see the third-party notices included [here](THIRD-PARTY-NOTICES.txt).
## References
- The [hexagonal coordinate system](src/Rebus/HexPoint.cs) and [layout](src/Rebus.Client/Layout.cs) are inspired by and based on [this](https://www.redblobgames.com/grids/hexagons/) article by [Amit Patel](http://www-cs-students.stanford.edu/~amitp/).
- The implementation of the [A* search algorithm](src/Rebus.Server/AStarSearch.cs) is based on [this](https://en.wikipedia.org/wiki/A*_search_algorithm) Wikipedia article.
- The [Roman numeral converter](src/Rebus.Server/NumeralSystems/RomanNumeralSystem.cs) is an improvement of [this](https://github.com/Humanizr/Humanizer/blob/main/src/Humanizer/RomanNumeralExtensions.cs) function attributed to [Jesse Slicer](https://github.com/jslicer).
- The implementation of [Durstenfeld\'s algorithm for the Fisher-Yates shuffle](src/Rebus.Server/FisherYatesShuffle.cs) is based on [this](https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle) Wikipedia article.
- The implementation of the [Julia set function](src/Rebus.Server/JuliaSet.cs) is based on [this](https://en.wikipedia.org/wiki/Julia_set) Wikipedia article.
