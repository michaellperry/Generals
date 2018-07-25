# What Two Generals Can Teach Us About Web APIs.

There are only two hard problems in distributed systems:

- 2 Exactly-once delivery
- 1 Guaranteed order of messages
- 2 Exactly-once delivery

[Mathias Verraes](https://twitter.com/mathiasverraes)

This becomes clear when you examine the [Two Generals Problem](https://en.wikipedia.org/wiki/Two_Generals%27_Problem).

## Two Generals

Two generals are camped outside of a city, laying siege.
They can only communicate by sending messages through enemy territorry.
They must reach an agreement in which both attack at the same time, or both abstain.
If only one attacks, then they will surely fail.

Can you come up with such a protocol?

Let's try.
Lest say that a general decides to attack tomorrow, but will only do so if he knows that the other general learns of his decision.
He sends a message "Attack" to the other general.
How will he know that the other general has received the message?
The messenger could be captured or killed.

He can try sending messenger after messenger.
But with no way of knowing that any messenger made it, he is still guessing as to whether the other general knows of his intentions.

So let's add a message to the protocol.
The other general upon receiving the "Attack" message will respond with "OK".
If he receives another "Attack", he will send out another "OK".
So when the "OK" makes it to the first general, he can stop sending messages.
He knows that the other will attack.

Or does he?
The other general doesn't know that the "OK" made it to the first general.
He knows the protocol, and realizes that the first general won't attack until it does.
And so he can clearly not choose to attack, as he might attack alone.

And so we can add a third message.
When the original general receives "OK", he can send out "Acknowledged".
Now when the other general receives "Acknowledged", he knows that the first has recieved the "OK" and will attack.

But is that true?
The first knows that the second won't attack until he receives "Acknowledged".
He doesn't know if that message ever made it through.
So he clearly cannot choose to attack on his own.

There is no protocol that solves the Two Generals problem.

## Simplifying the Problem

If we make a couple of assumptions, the problem becomes simpler:

1 One general decides to attack, and commits to that action
2 There is no deadline

If we make these assumptions, we can find a solution.
They introduce a time during which one general will attack, but the other will abstain.
The one who has decided will go ahead no matter what happens in the protocol.

But that risk is mitigated by the fact that we have eliminated the deadline.
In the actual battle scenario, this is unrealistic.
Surely, the attack will take place at *some* time in the future.
That future date *must* be a deadline.

But we'll make this assumption nonetheless and see if we can apply this reasoning to situations in which we *can* eliminate the deadline.

If we make these assumptions, then we find that we can solve the problem with three states and two messages. The states are:

- *Waiting* - The general will not attack.
- *Attacking* - The general will attack, but isn't sure about the other.
- *Assured* - The general will attack, and knows that the other will as well.

Both generals start in *Waiting*.
Then one decides that the time is right to attack and switches to *Attacking*.

Once in *Attacking*, a general periodically sends *Attack* messages.
If a general in *Waiting* recieves *Attack*, then he knows the intentions of the other general.
He switches to *Assured*, and responds with *OK*.

If a general in *Assured* receives another *Attack* message, there is no state change necessary.
He stays in *Assured* and sends out another *OK*.

When the general in the *Attack* state receives an *OK*, now he also knows the intentions of the other.
He switches to *Assured* and sends no more messages.

This is not a solution to the original problem.
This solution only works becaus of our simplifying assumptions.

## Eventual Consistency

These simplifying assumptions go by a different name.
In distributed systems, we call them "Eventual Consistency".
The system is allowed to be inconsistent for a time, and we impose no deadline upon it reaching consistency.
If we can convince our clients that inconsistency is OK for a short time, and that there is no deadline on what this "short time" happens to be, then we can solve the problem.
Without this set of simplifying assumptions, the problem is impossible.

Can I prove that it's impossible?
Indeed. That's called "The CAP Theorem".

## HTTP and CRUD

We often map the HTTP verbs `GET`, `PUT`, `POST`, and `DELETE` onto the operations Read, Update, Insert, and Delete.
This isn't the way we should think about them.
Thinking in this way leads to problems.
A better way to think about them is in terms of [three guarantees](https://en.wikipedia.org/wiki/Hypertext_Transfer_Protocol#Summary_table).

- Safe
- Idempotent
- No guarantee

`GET` is safe, because a well-behaved node will not change the state of the system in any observable way upon receipt of this request.
`PUT` and `DELETE` are idempotent, because a well-behaved node will only change state once, no matter how many times they are received.

Which leaves `POST`, offering no guarantees.
If we examine the Two Generals Problem, we find that idempotency is a valuable guarantee.
It is what keeps a general in the *Assured* state even when they recieve a second *Attack*.

Why should we not expect inserts to provide this guarantee?

## Make Inserts Idempotent

In the associated source code, you will find a branch called `before`.
This is a project before applying what we learned from the Two Generals.
In this example, we use `POST` to insert to-do lists.

If you switch to the branch `listid`, you will see that we've modified the API to use `PUT` instead to insert to-do lists.
It's OK, the specification allows this.
The client is providing the URL at which the resource will be hosted.
They are generating the ID.
Which brings us to the second piece of advice.

## Never Expose Internal IDs

We are far to quick to use the autoincrement ID of the 
