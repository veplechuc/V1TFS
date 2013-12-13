# TFS: Check-in and Build

There are a small set of things developers rarely forget to do. Two of those things are check-in and build. The ability to "hook" into these activities promises managers greater insight into development activities and developers less disruption of their day-to-day activities. The following are not the only things that PMs and Developers want, but represent a happy path that should be possible using their version control and build tooling.

## Prepare to Check-in Code

As a developer, I just spent 2 hours working in the code. Now, I want to write a check-in comment that references one or more workitems I have been working on to indicate the reason for my code change. I don't remember 5 digit numbers too well so I want it to be easy to find my workitems and reference them.

TODO: As a developer, I just spent 2 hours working in the code. Now, I want to write a check-in comment that updates the story I have been working on. I want to be able to update time, status, and owner using language that feels as natural.

## Navigate to a Referenced Story

TODO: As a developer, I just checked in a code change with a reference to a story. I can see the referenced number in my check-in comment and I want to go to that story in VersionOne to make some updates. I want it to be easy to navigate to that referenced number from my version control system.

## Find Workitems in a Build

As a developer, I just kicked off a build by making a code check-in. I want consumers of that build (like my tester and product owner) to be aware of the stories and defects that are in it. That way testers can better focus their testing efforts.

## Navigate to a Check-in

A tester picked up my build and was testing my story. She found a defect and wants me to fix it. As a developer, I want to start root cause analysis by looking at the check-ins related to my story.

## Traceability Audit

TODO: As a project manager on an agile project in an audit-heavy environment (CMMI, COBIT, SOx), I need to show where changes to product come from. I want to generate a report that connects code changes (check-ins) to the "who" and "why" in the form of stories and defects.
