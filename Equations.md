<!-- Copyright (c) 2021-2022 Ishan Pranav. All rights reserved. -->
<!-- Licensed under the MIT License. -->

# Equations
This article documents the mathematics implemented in this project for my own reference.
## Cantor pairing function
The [Cantor pairing function](https://en.wikipedia.org/wiki/Pairing_function#Cantor_pairing_function) $\pi\left(k_1,k_2\right)$ where $k_1,k_2\in\left\\{0,1,2,3,\dots\right\\}$ is defined

$$\pi\left(k_1,k_2\right)=\frac{\left(k_1+k_2\right)\left(k_1+k_2+1\right)}{2}+k_2,$$

$$\pi\left(k_1,k_2\right)=\frac{k_{1}^{2}+k_1+2k_1k_2+3k_2+k_{2}^{2}}{2}.$$

This function comes from [Wikipedia](https://en.wikipedia.org/wiki/Pairing_function#Cantor_pairing_function).
## Julia set function
Define a complex polynomial with a complex argument $c$:

$$f_c\left(z\right)=z^2+c.$$

Select an escape radius $R$ such that $R>0$ and $R^2-R\geq|c|$.

Let $f_{c}^{n}\left(z\right)$ represent the $n$-th iterate of $f_c\left(z\right)$ using [Newton\'s method](https://en.wikipedia.org/wiki/Newton%27s_method), then the filled Julia set is

$$K\left(f_c\right)=\left\\{z\in\mathbb{C}:\forall n\in\mathbb{N},|f_{c}^{n}\left(z\right)|\leq R\right\\}.$$

The [Julia set](https://en.wikipedia.org/wiki/Julia_set) $J\left(f_c\right)$ of the function is the boundary of $K\left(f_c\right)$. This function comes from [Wikipedia](https://en.wikipedia.org/wiki/Julia_set#Quadratic_polynomials).

## Rebus conflict resolution system
When a player attempts to invade a zone, the invading fleet and the defending fleet (all occupying units that did not retreat) are compared. This system is used when the two fleets do not contain the same number of units.

Let $i_0$ be the initial size of the smaller (minor) fleet. The number of units captured by the larger fleet ($c$) is

$$c=\max\left(1,\frac{i_0}{4}\right).$$

The remaining $d$ units are destroyed, leaving the $i$-sized smaller fleet empty:

$$d=i_0-c,$$

$$i=i_0-c-d,$$

$$i=0.$$

The larger fleet loses $d$ units but captures $c$ units, so its final size $j$ is

$$j=j_0+c-d,$$

$$j=j_0-i_0+2\times\max\left(1,\frac{i_0}{4}\right).$$
