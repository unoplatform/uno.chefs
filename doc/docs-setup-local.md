# How to setup the Uno Platform docs locally with the Chefs Recipe Book

The content of the Recipe Book is embedded as part of the Uno Platform docs using DocFx.

To test the Recipe Book follow these instructions:

1. Clone the main Uno Platform repo.
1. Checkout the `dev/chefs-recipe-book` branch.
1. Open the *doc\import_external_docs.ps1* script in an editor, and from the `external_docs` variable comment out all lines that represent a repo you're not interested in by prefixing it with a hash sign (`#`).
1. Open the *doc\import_extenal_docs_test.ps1* script in an editor, and do the same thing, but now add the following line to the collection:

    ```bash
    "uno.chefs" = "main"
    ```

    Feel free to replace `main` with a branch/commit hash of your choice.
1. Open a PowerShell CLI, navigate to the cloned *Uno* repo's *doc* folder and call the *import_external_docs_test* script:

    ```bash
    PS> cd unoplatform/uno/doc
    PS> .\import_external_docs_test
    ```

1. DocFx will run through the docs and will print out warnings and error messages found in the contents. This is a good chance to hunt for any errors in the docs.
1. The browser will launch the docs site, note the port number.
1. Some mis-designed objects can be ignored, these are applied from the production CSSs.
1. When done, come back to the CLI and press <kbd>Ctrl</kbd>+<kbd>C</kbd> to terminate the docs server.
