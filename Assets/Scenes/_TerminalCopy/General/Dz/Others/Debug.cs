//public static class Debug {
//    const int ALIGN_COLUMN = 47; // change this to move message column

//    public static void Log(params object[] messages) {
//        var trace = new StackTrace(true);

//        for (int i = 1; i < trace.FrameCount; i++) {
//            var frame = trace.GetFrame(i);
//            var method = frame.GetMethod();

//            if (method.DeclaringType?.Assembly.GetName().Name == "GodotSharp")
//                break;

//            string file = frame.GetFileName();
//            int line = frame.GetFileLineNumber();

//            string fileName = file != null
//                ? Path.GetFileName(file)
//                : method.DeclaringType?.Name + ".cs";

//            string prefix = $"[{fileName} @ {line} ; {method.Name}]";

//            var sb = new StringBuilder();

//            // align the message
//            sb.Append(prefix.PadRight(ALIGN_COLUMN));

//            for (int j = 0; j < messages.Length; j++) {
//                if (j > 0)
//                    sb.Append(" ");

//                sb.Append(messages[j]?.ToString());
//            }

//            GD.Print(sb.ToString());
//            return;
//        }

//        GD.Print(messages);
//    }


//    // public static void Log(params object[] messages) {
//    //     var trace = new StackTrace(true);

//    //     for (int i = 1; i < trace.FrameCount; i++) {
//    //         var frame = trace.GetFrame(i);
//    //         var method = frame.GetMethod();

//    //         // stop when entering Godot internals
//    //         if (method.DeclaringType?.Assembly.GetName().Name == "GodotSharp")
//    //             break;

//    //         string file = frame.GetFileName();
//    //         int line = frame.GetFileLineNumber();

//    //         string fileName = file != null
//    //             ? Path.GetFileName(file)
//    //             : method.DeclaringType?.Name + ".cs";

//    //         var sb = new StringBuilder();
//    //         sb.Append($"[{fileName} @ {line} ; {method.Name}] ");

//    //         for (int j = 0; j < messages.Length; j++) {
//    //             if (j > 0)
//    //                 sb.Append(" ");

//    //             sb.Append(messages[j]?.ToString());
//    //         }

//    //         GD.Print(sb.ToString());
//    //         return; // only print the first valid frame
//    //     }

//    //     GD.Print(messages); // fallback
//    // }

//    public static void CompleteLog(params object[] messages) {
//        var trace = new StackTrace(true);
//        var frames = new List<string>();

//        for (int i = 1; i < trace.FrameCount; i++) {
//            var frame = trace.GetFrame(i);
//            var method = frame.GetMethod();

//            if (method.DeclaringType?.Assembly.GetName().Name == "GodotSharp")
//                break;

//            string file = frame.GetFileName();
//            int line = frame.GetFileLineNumber();

//            string fileName = file != null
//                ? Path.GetFileName(file)
//                : method.DeclaringType?.Name + ".cs";

//            frames.Add($"{fileName} @ {line} ; {method.Name}");
//        }

//        var sb = new StringBuilder();
//        sb.Append("[");

//        for (int i = frames.Count - 1; i >= 0; i--) {
//            sb.Append(frames[i]);

//            if (i > 0)
//                sb.Append(" => ");
//        }

//        sb.Append("] ");

//        for (int i = 0; i < messages.Length; i++) {
//            if (i > 0)
//                sb.Append(" ");

//            sb.Append(messages[i]?.ToString());
//        }

//        GD.Print(sb.ToString());
//    }
//}