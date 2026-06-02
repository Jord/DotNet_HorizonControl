# DotNet_HorizonControl

## Merging and Deployment Strategy

> Please do not use `master`, `uat` or `development branches

This repository is configured with a single `main` branch that represents the latest deployable version of the software.

When making changes, developers should follow this workflow:

1. Create a new branch from `main` for their proposed changes.
2. Make the necessary changes in the new branch.
3. Test the changes locally to ensure they work as expected.
4. Commit the changes with a descriptive message.
5. Push the branch to the remote repository.
6. Open a pull request (PR) from the new branch targeting `main`.
7. A build will be triggered automatically, if the build fails fix the issues and push the changes again.
8. Once a green build is achieved, and your are confident with the changes, merge the PR to main.

### Which environments are deployed to?

At the moment `main` branch is currently deployed to **all** environments.

We are doing this to get quick feedback about any differences in configuration across the environments.
In future we will configure some deployment gates, so that only approved/tested builds are deployed to production,
but the concept will remain the same: `main` is our always our single latest version of the software.
