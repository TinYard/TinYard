# Branch Guidelines

## Master

`master` branch is a special branch. 

`master` branch should be treated as a release-only branch. This means it should only be merged into, and only from the `development` branch, but only once `development` is release ready.

`master` can also be merged into from [hotfix](#Hotfixes) branches.

## Issue related branches

Most branches should fall here.

Issue related branches are any branches that are created to resolve issues. These branches should be linked to an issue, and use keywords in their Pull Requests to signify this. E.g 'resolves #1'

The naming convention for these branches is dependent on the type of issue it is aiming to resolve. These branches should begin based on the main label of the issue, followed by a forward-slash and then an appropriate title - usually a shortened name of the issue.

All branch names should also be lower-case and spaces replaced with hyphens (-).

* Features/Enhancements: `feature/`
* Bugs: `bug/`
* Documentation: `documentation/`

For example - Let's say we want to work on Issue #7, Bundle Installing, we would create a branch called: `feature/bundle-installing` and in our Pull Request description we would put `resolves #7` and ensure that the Issue is linked with the Pull Request.

## Hotfixes

Hotfixes are another special kind-of branch. 

They should be named similar to [Issue related branches](#Issue-related-branches), where they have the prefix of `hotfix/` but they are not required to be related to an Issue - although this is helpful and recommended.

Hotfixes are only in dire cases where a severe/crippling bug has slipped into a release and needs immediate patching. This `hotfix` branch should stem off of the release commit on `master` that it relates to, and should merge back into `master` as another release.