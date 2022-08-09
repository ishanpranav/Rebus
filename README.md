<!-- Copyright (c) 2021-2022 Ishan Pranav. All rights reserved. -->
<!-- Licensed under the MIT License. -->

# Ishan Pranav\'s REBUS
A student-developed multiplayer space trading game. This project is a work in progress. Please see the details of the design [here](Design.md).
## API Documentation
The public API is documented [here](https://ishanpranav.github.io/rebus/pages/Rebus.html).
## License
This repository is licensed with the [MIT](LICENSE.txt) license.
## Attribution
This software uses third-party libraries or other resources that may be
distributed under licenses different than the software. Please see the third-party notices included [here](THIRD-PARTY-NOTICES.txt).
## References
- The [hexagonal coordinate system](src/Rebus/HexPoint.cs) and [layout](src/Rebus.Client/Layout.cs) are inspired by and based on [this](https://www.redblobgames.com/grids/hexagons/) article by [Amit Patel](http://www-cs-students.stanford.edu/~amitp/).
- The [list of star names](data/Stars.txt) and the [list of constellation names](data/Constellations.txt) come from the International Astronomical Union\'s [catalog of star names](https://www.iau.org/science/scientific_bodies/working_groups/280/) and [constellation database](https://www.iau.org/public/themes/naming_constellations/), respectively. The game map is randomly generated; the positions and names of stars are inaccurate by design.
- The [list of Roman deities](data/Planets.txt) comes from [this](https://en.wikipedia.org/wiki/List_of_Roman_deities) Wikipedia article.
- The formula for the [Cantor pairing function](src/Rebus/CantorPairing.cs) comes from [this](https://en.wikipedia.org/wiki/Pairing_function#Cantor_pairing_function) Wikipedia article.
- The implementation of the [A* search algorithm](src/Rebus.Server/AStarSearch.cs) is based on [this](https://en.wikipedia.org/wiki/A*_search_algorithm) Wikipedia article.
- [Durstenfeld\'s algorithm for the Fisher-Yates shuffle](src/Rebus.Server/FisherYatesShuffle.cs) is based on [this](https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle) Wikipedia article.
- The implementation of the [Julia set function](src/Rebus.Server/JuliaSet.cs) is based on [this](https://en.wikipedia.org/wiki/Julia_set) Wikipedia article.
- The [negative exponential distribution pseudo-random number generator](src/Rebus.Server/NegativeExponentialRandom.cs) is based on the discussion in [this](https://stackoverflow.com/questions/20385964/generate-random-number-between-0-and-1-with-negativeexponential-distribution) Stack Overflow thread.
- The [power set generator](src/Rebus.Server/PowerSet.cs) is based on the discussion in [this](https://stackoverflow.com/questions/19890781/creating-a-power-set-of-a-sequence) Stack Overflow thread.
