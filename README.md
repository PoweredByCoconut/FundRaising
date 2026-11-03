# Fund Raising Game

## Purpose

This project is meant to generate revenue for the ACM club at College of the Ozark's homecoming.

## Usage

This project is built with Godot 4.7.2 - .**NET**. Ensure you have the correct version before building or contributing.

## Planning

Tasks will coordinated through GitHub Issues for ease of use and integration.

## Contributing

This is the workflow for PR's, if this is not followed, your PR will be rejected:

- feature/your-feature:
  - This is the branch you work on and commit all your changes to.
- develop:
  - This branch holds the latest reviewed changes. Considered unstable, but should not have breaking bugs, should be
	updated from a PR from a feature / bugfix branch, but can be updated directly in certain situations
- RC/SEMVAR:
  - These branches are used to review all changes before a release, should be created from a commit on develop, can be
	directly pushed to
- main:
  - Holds the latest released version of the software, can only be updated from a PR from RC/SEMVAR
