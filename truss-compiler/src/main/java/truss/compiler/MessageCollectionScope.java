package truss.compiler;

import org.apache.commons.lang.Validate;

public class MessageCollectionScope {
    private MessageCollectionScope() {
    }

    private static final ThreadLocal<MessageCollection> CURRENT = new ThreadLocal<>();

    public static MessageCollection getCurrent() {
        return CURRENT.get();
    }

    public static void addMessage(Message message) {
        Validate.notNull(message, "message");

        MessageCollection messages = getCurrent();

        assert messages != null;

        messages.add(message);
    }

    public static Closeable create(MessageCollection messages) {
        Validate.notNull(messages, "messages");

        CURRENT.set(messages);

        return new Closeable() {
            @Override
            public void close() {
                CURRENT.remove();
            }
        };
    }
}
