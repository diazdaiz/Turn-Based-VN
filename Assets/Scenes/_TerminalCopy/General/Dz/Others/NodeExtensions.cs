//using Godot;

//public static class NodeExtensions {
//    public static T Instantiate<T>(this T original, Node parent = null) where T : Node {
//        T clone = (T)original.Duplicate();

//        parent ??= original.GetTree().CurrentScene;

//        parent?.AddChild(clone);

//        return clone;
//    }

//    public static Node Instantiate() {
//        return new();
//    }
//}