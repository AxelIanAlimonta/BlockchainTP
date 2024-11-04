// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

contract VotingSystem {
    struct Voter {
        bool hasVoted;
        uint256 candidateVoted; // ID del candidato por el que votó
    }

    struct Candidate {
        string name;
        uint256 voteCount;
    }

    mapping(address => Voter) public voters;
    Candidate[] public candidates;
    address[] public voterAddresses;

    // Constructor que inicializa los candidatos
    constructor() {
        // Candidatos precargados
        candidates.push(Candidate("Juan Ponce", 0));
        candidates.push(Candidate("Laura Silva", 0));
        candidates.push(Candidate("Pablo Morales", 0));
    }

    function vote(uint256 candidateId) public {
        require(!voters[msg.sender].hasVoted, "Ya has votado");
        require(candidateId < candidates.length, "Candidato invalido");

        voters[msg.sender] = Voter({
            hasVoted: true,
            candidateVoted: candidateId
        });
        candidates[candidateId].voteCount += 1;
        
        // Almacena la dirección del votante si es su primer voto
        voterAddresses.push(msg.sender);
    }

    function getCandidateVoteCount(uint256 candidateId) public view returns (uint256) {
        require(candidateId < candidates.length, "Candidato invalido");
        return candidates[candidateId].voteCount;
    }

    function getCandidateName(uint256 candidateId) public view returns (string memory) {
        require(candidateId < candidates.length, "Candidato invalido");
        return candidates[candidateId].name;
    }

    function getAllVoterAddresses() public view returns (address[] memory) {
        return voterAddresses;
    }

    function hasVoterVoted(address voterAddress) public view returns (bool) {
    return voters[voterAddress].hasVoted; 
    }

    function getVoterCandidate(address voterAddress) public view returns (uint256) {
    return voters[voterAddress].candidateVoted;
    }
}