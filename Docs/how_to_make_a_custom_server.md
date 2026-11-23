# Custom Server Assembly In The Editor

1. Add a server model.
2. In it's hierarchy, add a box collider which will represent the attach area (the area in which components will be grabbed for attachment). **Set it to `Is Trigger = True`**
3. Add `ServerHardwareSocket` to this box collider and set the server you added at 1 as the `Server` property.
4. In the original server object, add `CustomBuildServer`.
5. Customize the maximum number of components and component positions.