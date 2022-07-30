<!-- Copyright (c) 2021-2022 Ishan Pranav. All rights reserved. -->
<!-- Licensed under the MIT License. -->

# Design
Ishan Pranav\'s REBUS simulates a dialogue between a _player_ and their _microcomputer_ assistant. The player works with their microcomputer to issue orders to _units_ under their control.
## Units
All units are functionally identical: Each has the capacity to carry at most one unit of _cargo_  at any moment.

A player may order a unit to jettison its cargo at any time. Jettisoned cargo is destroyed.

Although each type of cargo is uniquely identified by its _mass_, this property does not affect a units\'s ability to carry it.
## Navigation
The game universe is a grid of hexagonal _zones of space_ which may be
- _empty_, containing no permanent features;
- _stellar_, containing a _star_; or
- _planetary_, containing an _inhabited_ or an _uninhabited planet_.[^1]

Units may not enter stellar zones.
### Exploration
A player may order a unit to travel to one of up to six zones adjacent to it.

_Wealth_ is a game currency stored virtually within the microcomputer. Every time wealth is added to a player\'s negative balance, one-tenth of the income is levied as an interest penalty and destroyed. Banker\'s rounding is used to approximate interest amounts to the nearest integer.

Attempting to move a unit into a zone incurs a fuel cost measured in wealth.

Exploration is blind: Any prior information about a destination and its contents may be incomplete, unreliable, or outdated until a unit enters and observes the zone.[^2]

A star is observed from all zones adjacent to its own.
### Autopilot
If a player has previously explored a zone, they may use _autopilot technology_ to automate a journey to it. The autopilot always travels the shortest route through non-stellar zones already visited by the player. If multiple "shortest" routes exist, then one is selected arbitrarily but deterministically. 

Even while autopilot is enabled, a unit must enter and exit each intermediate zone on the route before reaching its intended destination.
## Interception
When a player\'s unit attempts to enter a zone _occupied_ by an _adversary\'s_ unit,[^3] then the adversarial unit _intercepts_ the player\'s, which must stop. The autopilot is immediately disabled.

Each occupying unit maintains an _interception stance_ assigned by the owning player. When a player enters an adversary-occupied zone, the occupant\'s local stance determines the consequences.[^4]

### Passive stance
A player may order a unit to remain _passive_ by designating an adjacent non-stellar _sanctuary_ zone to which they will retreat if an adversary attempts to enter and occupy the zone.

If the sanctuary zone is occupied by any adversary, then the attempt to retreat fails and the passive unit defaults to _defensive_ behavior.

### Defensive stance
Units ordered to defend their zone attempt to prevent adversaries from entering it.

The _sizes_ of the _occupying_ and _invading_ fleets are given by the number of units they contain. The fleet with fewer is the _minor_ fleet; the other is the _major_ fleet.

#### Standoff
If the occupying and invading fleets are of the same size, a _standoff_ occurs: The invading fleet holds its position and may not enter the adversary-occupied zone.

#### Conflict
If no standoff occurs, each fleet loses a number of units $d$ based on the minor fleet\'s initial size $m_0$.

Units are destroyed in descending order based on the mass of their cargo.[^5]

The major fleet captures the remaining units in the minor fleet. A capture occurs in every combat situation. Ownership of the $c$ captured units changes, and they join the initial major fleet of size $j_0$.

The final size of the major fleet $j$ is given by the following process:

$$j=j_0+c-d.$$

$$c=\max\left(1,\frac{i_0}{4}\right).$$

$$d=i_0-c.$$

$$j=j_0-\left(i_0-c\right)+c.$$

$$j=j_0-i_0+\left(2\times\max\left(1,\frac{i_0}{4}\right)\right).$$

Therefore, the final minor fleet of size $i$ is always empty:

$$i=i_0-c-d.$$

$$i=0.$$

## Commerce
A player with a unit in a zone may conduct _commerce_ with any planet within that zone by exchanging wealth for _commodities_ stored as cargo.[^6]

At an inhabited planet, a player may _sell_ commodities being imported by the planet and _purchase_ commodities being exported by the planet.

Each purchase increases the unit _selling price_ that the exporting planet requests in exchange for the commodity; each sale decreases the unit _purchasing price_ that the importing planet is willing to pay for the commodity.

The quantities supplied and demanded by each planet are unlimited. The amount that a player sells is limited only by their cargo; the amount that a player purchases is limited only by their wealth and the total capacity of their fleet. 

At an uninhabited planet, a player may _accept_ cargo and _deposit_ cargo.[^7]

## Future considerations
Potential planetary facilities under consideration include
- **terminals** offering passengers as an alternative form of cargo (this facilitates colonization – players may accept passengers from inhabited planets and deposit them onto uninhabited ones to inhabit them or evacuate one planet and migrate its population to another; population size may have an impact on the economy, and passengers cannot be jettisoned (passengers have zero mass),
- **banks** that allow a player to accept or deposit wealth into a common pool for any player to collect (this facilitates contracts, transfers of wealth between players, and races between players to arrive at the bank to withdraw first),
- **shipyards** that allow players to purchase units,
- **embassies** that allow players to negotiate government intervention in the planet\'s economic system (via price floors and ceilings), and
- **insurance agencies** that mitigate the risks of exploration and conflict.

[^1]: \"Planet\" is defined more broadly than in astronomy. It refers to any non-stellar celestial body (for example, a moon, dwarf planet, space station, satellite, or asteroid).

[^2]: Empty units might be used as scouts to avoid entering traps. Weaker players might exploit the vast universe size to escape stronger players\' dominions. A \"hide-and-seek\" mechanic might emerge.

[^3]: No feature exists formalizing alliances. Every player is an adversary of every other player, regardless of informal agreements. No distinction exists between human- and artificial-intelligence-controlled players.

[^4]: Only one player may occupy a region at a time. Interception stances are indiscriminate: They do not respect informal agreements, so deception is possible. No conflict occurs without both parties\' consent. By entering an explored zone, the invading player is acting aggressively and must fance any potential resistance. Meanwhile, the stance system ensures that the defender decides whether a conflict will occur. When a player assigns asynchronous stances to their units, the orders remain effective even while the player is disconnected from the server. Thus, the system protects inactive players from harrassment. For the aggressive player, the risk of being ambushed by a superior force deters exploration. At the same time, exploration becomes inevitable as local markets become unprofitable.

[^5]: Since players and planetary markets determine demand, the mass of a unit of cargo is not necessarily associated with its value. And, since units carrying the most massive cargo are destroyed first, players might reserve some units to carry less expensive, more massive, cargo to avoid the destruction of more valuable commodities. This system exhibits no randomness and can indirectly facilitate cargo exchanges.

[^6]: No feature exists allowing one player to directly transfer wealth to another. Players might simulate trades via cargo deposits on uninhabited planets. There is an incentive to discover remote uninhabited planets for undisturbed bartering. Arranged meetings encourage traps and betrayals, limiting excessive player cooperation and maintaining fair competition.

[^7]: Piracy is possible by accepting unclaimed cargo before its intended recipient. A \"treasure hunt\" mechanic might emerge.
