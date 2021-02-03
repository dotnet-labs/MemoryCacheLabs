# Memory Cache Lab

## [Medium Article: IMemoryCache: Immutable Collections and Unit Tests](https://codeburst.io/imemorycache-immutable-collections-and-unit-tests-cfac7b389a5)

There are many introductory articles talking about using the `IMemoryCache` to store data in the memory of the webserver. However, few of them have mentioned how to ensure the consistency of cached values at runtime. In other words, if not designed carefully, the cached values could be mutated accidentally in code.

In this article, I will first present an example cache service that allows us to mutate a cached entry, which is an undesired outcome in most cases. To improve the implementation, next, I will show you a way to create immutable collections as cache objects by using `IReadOnlyList<T>` and `record`.

![immutable-cache](immutable-cache.png)
