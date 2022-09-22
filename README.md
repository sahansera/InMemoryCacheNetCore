# In-Memory Caching .NET Core 6 with IMemoryCache
[![Documentation](https://img.shields.io/badge/documentation-yes-brightgreen.svg)](sahansera.dev)
[![.NET](https://github.com/sahansera/InMemoryCacheNetCore/actions/workflows/dotnet.yml/badge.svg?branch=master)](https://github.com/sahansera/InMemoryCacheNetCore/actions/workflows/dotnet.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](#)
[![Twitter: _SahanSera](https://img.shields.io/twitter/follow/_SahanSera.svg?style=social)](https://twitter.com/_SahanSera)

## Intro 👋

This project leverages the IMemoryCache that's shipped as part of .NET/.NET Core SDKs to achieve in-memory caching specifically in monolithic environments. If you are looking for a distributed caching approach, then, my [other project](https://github.com/sahansera/DistributedCacheAspNetCoreRedis) would be more suitable for you.

I have also [blogged](https://sahansera.dev/in-memory-caching-aspcore-dotnet/) with a full explanation on how this is achieved.

## Architecture 🏗
![](./Content/caching-2.jpg)

1. User A makes a request to our web service
2. In-memory cache doesn’t have a value in place, it enters in to lock state and makes a request to the Users Service
3. User B makes a request to our web service and waits till the lock is released
4. This way, we can reduce the number of calls being made to the external web service. returns the response to our web service and the value is cached
5. Lock is released, User A gets the response
6. User B enters the lock and the cache provides the value (as long it’s not expired)
7. User B gets the response

## Usage 🚀

Open up in your favorte editor and do a `dotnet run` at the root of the project.

## Questions? Bugs? Suggestions for Improvement? ❓

Having any issues or troubles getting started? [Get in touch with me](https://sahansera.dev/contact/) 

## Support 🎗

Has this Project helped you learn something new? or helped you at work? Please consider giving a ⭐️ if this project helped you!

## Share it! ❤️

Please share this Repository within your developer community, if you think that this would make a difference! Cheers.

## Contributing ✍️

PRs are welcome! Thank you
