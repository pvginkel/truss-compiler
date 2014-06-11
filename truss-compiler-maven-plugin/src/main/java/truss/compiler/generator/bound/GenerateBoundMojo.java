package truss.compiler.generator.bound;

import org.apache.maven.plugin.AbstractMojo;
import org.apache.maven.plugin.MojoExecutionException;
import org.apache.maven.plugins.annotations.LifecyclePhase;
import org.apache.maven.plugins.annotations.Mojo;
import org.apache.maven.plugins.annotations.Parameter;

import java.io.File;

@Mojo(name = "generate-bound", defaultPhase = LifecyclePhase.GENERATE_SOURCES)
public class GenerateBoundMojo extends AbstractMojo {
    @Parameter(property = "definition", required = true)
    private File definition;

    @Parameter(defaultValue = "${project.build.directory}/generated-sources/truss-compiler-bound", property = "outputDir", required = true)
    private File outputDirectory;

    public void execute() throws MojoExecutionException {
        try {
            new GenerateBound().generate(definition, outputDirectory);
        } catch (Throwable e) {
            throw new MojoExecutionException("Could not generate sources", e);
        }
    }
}
