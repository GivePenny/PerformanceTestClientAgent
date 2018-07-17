# Performance Test Client Agent

## Overview and Scope

Performance Test Client Agent is a lightweight load-generating class library for use in performance testing.  It's intended to be referenced by a console application that is deployed onto a container orchestration platform.  The platform ideally should have the capability to start multiple running instances of this agent (e.g. Kubernetes pods).

## Example context architecture

This component is intended to be used as part of the following example set:
1. A microservice deployed to a Kubernetes cluster, for example into Azure Kubernetes Service.  This is the service under test.
2. Microservice monitoring dashboards, logs and metrics visualisation.  We use Log Analytics, Application Insights and Azure Portal Dashboards.  These are the same dashboards / monitoring solutions that serve operational support requirements for the microservice.
3. Performance Test Load generator (this component).
4. Performance Test Results Verifier.  This is a custom unit test project that queries the monitoring database after the performance test has finished.  We query Log Analytics for metrics captured during the performance test and the "unit" tests make assertions to verify performance is as expected.

This component has no dependency on Kubernetes or Service Fabric and is a simple .NET Standard class library.

## Example context usage

The following steps could be set up by a build and release system, we use VSTS:

1. A build to produce a Docker Container for the microservice and another Docker Container for the performance test agent that references this library.
2. A release that deploys the two containers into a test cluster (or into different clusters for E2E perf testing), instructing the hosting platform to start multiple copies of the performance test agent.
3. A release delay task whilst the CI perf test is running (on VSTS this can be an agentless task, meaning no builds/releases are blocked)
4. A release task that executes the Results "unit" tests (i.e. VSTS marks the release as failed if perf test assertions fail)

## Usage

TBD: Add example web api, perf test console application and k8s YML files