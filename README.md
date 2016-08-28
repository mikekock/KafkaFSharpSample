## Summary
This is a sample project that uses F# and Kafka to publish events to a topic and then consume those
events using a separate consumer process.

The project was built using Xamarin and Mono on a Mac but will also build and run using Visual Studio on Windows.
It targets .NET 4.5.1 in order to run on Mono using the rdkafka NuGet package.

The project generates events to Kafka simulating the creation of items, changing the description and deleting items. Item IDs are just random numbers
between 0-9. These events can then be consumed by other processes which currently just display the basic info from
each event on the console.
 
## Usage

Start one ore more consumers by running:

```
$ mono KafkaConsumer.exe [ConsumerGroupName]
```

Where [ConsumerGroupName] is the name of the Kafka consumer group you want the consumer to be a part of.
Multiple consumer processes can be launched using the same ConsumerGroupName of choice and the consumption load
will be automatically balanced between the consumers based on the key. The key is the item ID which is a random
number between 0-9.

Additional consumer processes using a different ConsumerGroupName can also be launched to process the same events for
different purposes. 

Then to generate events, run:

```
$ mono KafkaProduer.exe [ItemCount]
```

Where [ItemCount] is the number of items to simulate creating/updating/deleting.


## Author & license


For licensing info see LICENSE file in project's root directory.
