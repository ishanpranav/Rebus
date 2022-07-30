<!-- Copyright (c) 2021-2022 Ishan Pranav. All rights reserved. -->
<!-- Licensed under the MIT License. -->

# Design
Ishan Pranav\'s REBUS is a multiplayer space trading game. Each player is either operated by a human or a computer opponent.
## Units
Players issue orders to units (spacecraft) under their control in real time: Some execute immediately, and others are asynchronous.
## Cargo
Cargo types are uniquely identified by their masses. Each unit can carry a single container of cargo, regardless of its mass. A player can order a unit to jettison (and destroy) its cargo at any time.
## Currency
The universal currency, credits, represents the unit price of fuel.
## Navigation
The game universe is a grid of hexagonal zones. In addition to an unlimited number of units, each zone can contain a star, an inhabited planet, an uninhabited planet,[^1] or no permanent features. Each attempted move costs one credit per unit per zone.
### Exploration
A player can order a unit to blindly explore one of up to six neighboring zones. Any information about the destination and its contents can be incomplete, unreliable, or outdated until a unit visits it.[^2] Units cannot visit stars, but can observe them from any of their neighbors.
### Autopilot
If a player has previously explored a zone, they may enable the autopilot to automate a journey to it. The autopilot always travels the shortest route through non-stellar zones already visited by the player. If multiple "shortest" routes exist, then one is selected arbitrarily but deterministically. Even while autopilot is enabled, each unit visits every zone on the route before reaching its intended destination.
## Conflict
A player can order a unit to retreat from future battles or to defend its zone. The player can issue such orders before (but not during) a conflict. A retreat order assigns a unit a sanctuary zone - a non-stellar neighbor which the player has previously explored.

If one player\'s unit attempts to visit a zone that another player occupies,[^3] it is intercepted. Before entering the zone, it must stop and disable the autopilot.

Each occupying unit ordered to retreat travels to its sanctuary zone, unless another player already occupies it. In that case, the retreat action fails: The unit defaults to defensive mode and does not incur a fuel charge for the attempt.

The defending fleet contains all units in defensive mode, while the invading fleet contains all the units attempting to enter the zone.

If the invading and defending fleets contain the same number of units, a _standoff_ occurs: The invading fleet holds its position and does not enter the occupied zone.

Otherwise, each fleet loses a number of units $d$ based on the smaller fleet\'s initial size $i_0$. Units are destroyed in descending order based on the masses of their cargo.[^5]

The larger fleet captures $c$ remaining units in the smaller fleet, and they join the larger fleet whose initial size is $j_0$.

The final size of the larger fleet $j$ is given by the following process:

$$j=j_0+c-d.$$

$$c=\max\left(1,\frac{i_0}{4}\right).$$

$$d=i_0-c.$$

$$j=j_0-\left(i_0-c\right)+c.$$

$$j=j_0-i_0+\left(2\times\max\left(1,\frac{i_0}{4}\right)\right).$$

Therefore, the smaller fleet of size $i$ is always eliminated after a conflict:

$$i=i_0-c-d.$$

$$i=0.$$

## Commerce
Units can trade with planets within their zones.[^6]

At inhabited planets, units can sell commodities that the planet imports and purchase commodities that it exports. Each purchase increases the unit selling price that the exporting planet requests in exchange for a commodity, and each sale decreases the unit purchasing price that the importing planet offers for it.

The quantities supplied and demanded by each planet are unlimited. The amount that a player sells is limited only by their cargo; the amount that a unit purchases is limited only by its cargo capacity and a player\'s wealth.

At uninhabited planets, units can accept and deposit cargo.[^7]
## Future considerations
Potential planetary facilities under consideration include
- **terminals** offering passengers as an alternative form of cargo (this facilitates colonization – players may accept passengers from inhabited planets and deposit them onto uninhabited ones to inhabit them or evacuate one planet and migrate its population to another; population size may have an impact on the economy, and passengers cannot be jettisoned (passengers have zero mass),
- **banks** that allow a player to accept or deposit wealth into a common pool for any player to collect (this facilitates contracts, transfers of wealth between players, and races between players to arrive at the bank to withdraw first),
- **shipyards** that allow players to purchase units,
- **embassies** that allow players to negotiate government intervention in the planet\'s economic system (via price floors and ceilings), and
- **insurance agencies** that mitigate the risks of exploration and conflict.

[^1]: \"Planet\" extends to any non-stellar celestial body (for example, a moon, dwarf planet, space station, satellite, or asteroid).

[^2]: Players might use empty units as scouts to avoid entering traps. Weaker players might exploit the vast universe size to escape stronger players\' dominions. A \"hide-and-seek\" mechanic might emerge.

[^3]: No feature exists formalizing alliances. Every player is an adversary of every other player, regardless of informal agreements.

[^4]: Only one player may occupy a region at a time. Interception stances do not respect informal agreements, so deception is possible. Once a conflict is in progress, the interception stances cannot be changed. No conflict occurs without both parties\' consent. By entering an explored zone, the invading player is acting aggressively and must face any potential resistance. Meanwhile, the stance system ensures that the defender decides whether a conflict will occur. When a player assigns asynchronous stances to their units, the orders remain effective even while the player disconnects from the server. Thus, the system protects inactive players from harassment. For the aggressive player, the risk of being ambushed by a superior force deters exploration. At the same time, exploration becomes inevitable as local markets become unprofitable.

[^5]: Since players and planetary markets determine demand, the mass of a unit of cargo is not necessarily associated with its value. And, since units carrying the most massive cargo are destroyed first, players might reserve some units to carry less expensive, more massive, cargo to avoid the destruction of more valuable commodities. This system exhibits no randomness and can indirectly facilitate cargo exchanges.

[^6]: No feature exists allowing one player to directly transfer wealth to another. Players might simulate trades via cargo deposits on uninhabited planets. There is an incentive to discover remote uninhabited planets for undisturbed bartering. Arranged meetings encourage traps and betrayals, limiting excessive player cooperation and maintaining fair competition.

[^7]: Piracy is possible by accepting unclaimed cargo before its intended recipient. A \"treasure hunt\" mechanic might emerge.
