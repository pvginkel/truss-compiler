package truss.compiler;

import java.util.*;

public class CollectionUtils {
    public static <T> List<T> copyList(List<T> list) {
        if (list == null || list.size() == 0) {
            return Collections.emptyList();
        }

        // Verify that there are no null items. This should only happen when there were already errors. If this is
        // the case, we remove them here so parsing can continue; kind of.

        for (int i = list.size() - 1; i >= 0; i--) {
            if (list.get(i) == null) {
                assert MessageCollectionScope.getCurrent().hasMessages();

                list.remove(i);
            }
        }

        return Collections.unmodifiableList(new ArrayList<>(list));
    }
}
