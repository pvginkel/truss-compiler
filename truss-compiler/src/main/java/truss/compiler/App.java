package truss.compiler;

public class App {
    public static void main(String[] args) {
        try {
            TrussArguments arguments = new TrussArguments(args);

            // TODO: Not yet implements.

            assert arguments.isNoCorLib();
            assert arguments.getLink().size() == 0;

            System.exit(new Builder(arguments).build());
        } catch (Throwable e) {
            e.printStackTrace();
        }
    }
}
