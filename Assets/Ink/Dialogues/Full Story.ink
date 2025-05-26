// === ROUND 1 START ===

->Round_1
// tags contain 
//#speaker: Male, Female, Narrator
//#SFX:
//#expression:
//#character: Killer, You
//#effect

//switch cases

//{progress:
//- 0: ->Round_1
//}
//- 1: ->chapter_1
//- 2: ->chapter_2
//- 3: ->chapter_3
//- 4: ->chapter_4
//- 5: ->chapter_5

//}

/* ---------------------------------

   Functions

 ----------------------------------*/
CONST INITIAL_SWING = 1001

=== function swing_count(x) 
    ~ return (upness(x) + downness(x)) - 2
=== function swing_ready(x) 
    ~ return swing_count(x) >= 2
=== function raise(ref x)
	~ x = x + 1000
=== function elevate(ref x)
    ~ raise(x)
    ~ raise(x)
    ~ raise(x)
=== function lower(ref x)
	~ x = x + 1 
== function ditch (ref x) 
    ~ lower(x) 
    ~ lower(x) 
    ~ lower(x)
=== function demolish(ref x)
    ~ x = x + 20
=== function escalate(ref x)
    ~ x = x + (20 * 1000)
=== function upness(x)
	~ return x / 1000

=== function downness(x)
	~ return x % 1000

=== function high(x)
    ~ return (1 * upness(x) >= downness(x) * 9)

=== function up(x)
	~ return swing_ready(x) && (4 * upness(x) >= downness(x) * 6)

=== function down(x)
	~ return swing_ready(x) && (6 * upness(x) <= downness(x) * 4)
	
=== function low(x)
	~ return swing_ready(x) && (9 * upness(x) <= downness(x) * 1)
	
=== function mid(x)
    // If the swing isn't ready this returns true 
    // Because "up is false and down is false"
    ~ return (not up(x) && not down(x))


/* ---------------------------------

   Global Variables

 ----------------------------------*/

VAR progress = ""

VAR trust = INITIAL_SWING
VAR delusion = INITIAL_SWING

//var for choices

VAR stabbed = false
VAR refuseToStab = false

//EXTERNAL OnRoundEnd(roundNumber)
//EXTERNAL OnRoundStart(roundNumber)

LIST ending = (none), A, B, C, D


//
// Content
//

 === Round_1

//elevator_intro 
#SFX: Ding
#background: ElevatorOpen
#speaker:Narrator
The elevator dings.
#pause(1.0)

You step into the elevator, the soft hum of machinery buzzing faintly beneath your feet. 
#pause(1.0)

It's cold. Sterile. Like the air is waiting for something to happen.

#pause(1.0)

The doors begin to close. But just before they shut—

#Background: HandOnDoor
A hand stops them.

#pause(0.5)
#walk sound
#background man foot walk in
And a man steps in.

 ->chapter_1
 === chapter_1
~ temp confidence = 0
//describe_man 
//~ OnRoundEnd(1)

#character: Killer
#expression: NeutralLookAway
#background look man down to shoulder and stop
The man's average. Tallish. Pale. Clean suit. Polished shoes.

But there’s something... off.


*   [look away] You keep your gaze on the floor. The fluorescent lights reflect in the polished steel.
    ~ confidence = 0

    
*   [stare at him] You meet his eyes. His eyes don’t quite match his smile. 
    Something sharp glints in them. Not metal. Something worse.
    ~ confidence = 1

  
-



 ->chapter_2
 === chapter_2
~ temp confidence = 0

//small_talk_start 


#speaker: Narrator
The elevator begins its slow descent.
The silence stretches.
Too long.
He finally speaks.

#speaker: Male
#character: Killer
#expression:Smile1
"Going down?"

#speaker: Narrator
You didn’t answer right away. Something in your throat locks.
 #speaker:Female

*   {confidence <= 0}[nervous smile] "Y-yeah. Just... just heading down."
    ~ confidence = 0 

*   {confidence >= 0}[bluntly]  "Yes."
    ~ confidence = 1
    
*   {confidence > 0}[make a joke]  "Only way to go from the top, right?"
    ~ confidence = 2
 


-
#speaker: Male
#character: Killer
#expression:SmileLookAway 
"Suppose so."  
"Funny thing, these little boxes."
"Tiny worlds. No escape. "
"Just you and... whoever walks in."

#speaker:Narrator
#character: Killer
#expression:NeutralLookAway 
He pauses. Like he’s letting the words hang in the air for too long.
...
#expression:NeutralLook 
Like he's watching them sink into your skin.
#speaker:Female

*   {confidence == 2 or confidence == 0}[awkward laugh]  "Yeah... elevators are weird."

    
*   {confidence <= 1}[tense]  "What do you mean by that?"
    ~lower(trust)
 
    
*   {confidence >= 1}[joking]  "Sounds like a setup for a horror movie."
    ~ raise(trust)
      
    
-
#speaker: Male
#character: Killer
#expression:SmileLook
"Horror... hmm. Maybe."
"Or just a moment where everything slips."
"When you realize you were never alone to begin with."

#speaker:Narrator
You laugh nervously, but it dies in your throat.


 ->chapter_3
 === chapter_3
//delusion_hint 

#speaker:Narrator

There’s something wrong.

His reflection in the elevator mirror behind him—it doesn’t move when he does.

You blink. 

Look again.

It’s back to normal.

...Was it?

*   [rub your eyes] You blink a few more times, heart thudding. Maybe you’re tired.
 ~ lower(delusion)
    
*   [look again]  You focus on the reflection. It stares back. Eyes too wide.
~ raise(delusion)

*   [step back slightly]  You shift away, gripping your bag tighter.

-

//checking variables
//(Trust - Up: {upness(trust)}, Down: {downness(trust)})
//(Delulu - Up: {upness(delusion)}, Down: {downness(delusion)})


 ->chapter_4
 === chapter_4

//voice_in_head 


#show_expression("paranoid")
#speaker: Narrator
A whisper curls in your mind. Soft. Familiar.

"He’s not real. He’s not. Not this time."

You shake your head. The whisper is gone.

The man’s still smiling. But his eyes... they flicker for just a moment.  
Black, like a void.

Then they’re normal again.

#speaker: Male
#character: Killer
#expression:NeutralLook  
"You alright? You look pale."

#speaker: Female

*   [lie]  "Just tired. Didn’t sleep well."
    ~ lower(trust)
*   [avoid]  "I’m fine."
    ~mid(trust)
*   [truth]  "Do you ever feel like someone’s lying to you? Even if they say all the right things?"
    ~ raise(trust)
-
#speaker: Male
#character: Killer
#expression:SmileLook  

#pause(1.0)
"All the time."

//checking variables
//(Trust - Up: {upness(trust)}, Down: {downness(trust)})
///(Delulu - Up: {upness(delusion)}, Down: {downness(delusion)})


 ->chapter_5
 === chapter_5
 
#speaker: Narrator
The elevator dings.

You’re almost at the lobby.

He steps closer. 
Just slightly. Almost imperceptibly.

But you notice.

#speaker: Male
#character: Killer
#expression:Smile1
"I think we'll be seeing more of each other."

#speaker: Female

*   {down(trust)} [freeze]  You don't respond. Can't.

*   {down(trust) or mid(trust)}[respond]  "I... hope not."

*   {mid(trust) or up(trust)}[fake a smile] "Looking forward to it."
    
-
#speaker: Narrator
The elevator doors open.

He steps out, but just before turning the corner, he glances back.

And he *winks*.

#show_expression("disturbed")
#pause(1.0)

You don’t remember pressing the emergency stop, but the elevator halts again.

You’re alone.

And yet…  
you still feel watched.


#add a cctv angle here
//~OnRoundEnd(1) 

~ progress = "Round_1_End"
-> DONE


// === ROUND 2 START ===


=== ROUND_2 ===
#pause_scene
#show_scene("elevator_dim")
#set_expression("neutral")

You step into the elevator once more. The air feels heavier today, the fluorescent lights casting a sickly hue.

#pause(1.0)

The doors begin to close, but a familiar hand halts their motion.

#pause(1.0)

He steps in.

-> describe_man_round2

= describe_man_round2 
#show_character("Man", "smile_neutral")

His suit is immaculate, but there's a subtle smear on his cuff. His eyes meet yours, and for a fleeting moment, they seem... hollow.

#speaker:Male
"Back again. Fate has a peculiar sense of humor, doesn't it?"
#speaker:Female
*  [forced smile]
    "Seems like it."
*   [uneasy]
    "Do you always ride this elevator?"
*   [silent]
    #speaker:Narrator
    You nod, avoiding his gaze.

-
#speaker:Male
"Elevators are like confessionals, don't you think? Small spaces where truths slip out."

-> elevator_ride_round2

= elevator_ride_round2 
#speaker:Narrator
The elevator hums as it descends. The floor numbers flicker erratically.

#speaker:Male
"Sometimes, I wonder if we're going down at all. Maybe we're just stuck, reliving the same moments."
#speaker:Narrator
You clutch your bag tighter, fingers brushing against something cold and metallic inside.
#speaker:Female
*   [inquire] "What do you mean by that?"

*   [defensive] "Are you implying something?"
    ~lower(trust)
    
*   [change subject] "Do you work in this building?"
-
#speaker:Male
#(smiling)
"Work? I suppose you could call it that. I observe, listen, understand."
#speaker:Narrator
His gaze intensifies, piercing through you.

#speaker:Male
"Tell me, do you ever feel like you're being watched?"
#speaker:Female
*   [truthful] "Yes... lately, more than ever."
    ~raise(trust)
*   [lie] "No, not really."
    ~lower(trust)
*   [deflect] "That's a strange question."
    ~mid(trust)
-
#speaker:Male
"Is it? Perhaps we're all just mirrors, reflecting each other's truths and lies."

-> delusion_build_up

= delusion_build_up 
#show_expression("disturbed")
#speaker:Narrator
The elevator's lights flicker, and for a moment, the man's reflection doesn't match his movements.

A whisper echoes in your mind: "He's not real. End it."

You shake your head, trying to dispel the voice.

#speaker:Male
"Are you alright? You seem... tense."
#speaker:Female
*   [confront] "Who are you really?"
    #speaker:Male
    (chuckles)
    "Whoever you think I am."
*   [deflect] "Just tired, that's all."
    #speaker:Male
    (chuckles)
    "Aren't we all?"
*   [accuse] "You're lying to me."
    #speaker:Male
    (chuckles)
    "Aren't we all?"

-


-> climax_round2

= climax_round2 
#speaker:Narrator
...
The elevator jerks to a halt between floors.

The lights dim.

Your hand moves on its own, reaching into your bag, fingers wrapping around the cold handle of a knife.

#speaker:Male
"Sometimes, the only way to see the truth is to cut away the lies."
#speaker:Narrator
*   [stab]
    <>Without hesitation, you lunge forward, plunging the knife into his chest.
*   [hesitate]
    <>You waver, but the voice in your head screams, and you act.
    
-

#blood splash sound
Blood splatters across the elevator walls.

The man's eyes widen in shock, then fade.

Suddenly, the elevator doors are pried open.

Police officers swarm in, weapons drawn.

OFFICER:
"Drop the weapon! Hands where we can see them!"

You release the knife, hands trembling.

As you're restrained, you glance at the man's lifeless body, a pool of blood spreading beneath him.

#show_expression("shocked")
#pause(2.0)
...
#speaker:Female
"He's...n8T ReAl...?"
#speaker:Narrator
The elevator doors close.

Darkness.

-> END


=== ROUND_3 ===
#speaker:Narrator
The elevator dings open again. Same floor. Same hallway. Same flickering light overhead. But this time, your chest feels tight—too tight.

Something’s wrong. Again. You remember this. You *REMEMBER* this.

You step in. 
He’s already there this time. 
Waiting. Smiling.

#speaker:Male
“Oh. You again.”
#speaker:Narrator
He tilts his head. That same polite nod. But there’s something behind his eyes this time. Or… maybe you’re finally seeing it.


You can feel your breathing gets uneven.

        *   [I stabbed him] <>
            I stabbed him last time. Didn’t I? 
            You clutch your stomach. You can still feel the resistance of the blade… or is that your imagination?
           
            
        *   [It's not real] <>
            No. That wasn't real. I wouldn't do that.
            You shake your head. No blood. No body. It was just in your mind. You never touched him.
            ~ raise(delusion)

        *   [He deserved it.] <>
             You stare at him, jaw clenched. The memory fuels you, not frightens you.
           

        *   [I’m not sure what’s real anymore…] 
        It’s slipping. Your sense of time. Of truth. Of self. You grip the elevator railing just to ground yourself.
        ~ elevate(delusion)
            
         -
         
-> man_speaks


= man_speaks 
#speaker:Male
“You look pale. Are you alright?”

    *   [Say nothing.]
        -> stare_silence
    *   [“I know who you are.”]
        -> accuse_him
    *   [“Why do you keep looking at me like that?”]
        -> confront_paranoia

= stare_silence 
#speaker:Narrator
You stay silent. He watches you longer than necessary, then shifts his weight, like a predator getting comfortable.
    -> floor_counter

= accuse_him 
~ lower(trust)
#speaker:Narrator
Your voice is hoarse.
#speaker:Female
“I know who you are.”
“You followed me. In all three rounds. You knew.”

#speaker:Male
"..."
“I think you’re mistaking me for someone else.”

    *   [“No. I’m not.”]<>
        -> push_accusation
    *   [Back off. Stay quiet.]
        -> stare_silence

= push_accusation 
~ lower(trust)
#speaker:Female
“No. I’m not.”
#speaker:Narrator
Your hand curls at your side. You *want* to believe he’s lying. Because if he’s not…
    -> floor_counter

= confront_paranoia 
~ raise(delusion)
#speaker:Female
“Why do you keep watching me like that?”
#speaker:Male
 “I’m… not? You’ve looked on edge since you walked in.”

    *   [“You’re lying.”]
        -> push_accusation
    *   [“...Sorry. Maybe I’m just tired.”]
        -> self_doubt

= self_doubt 
~ raise(trust)
You feel the heat rising to your face. Maybe it really is you. Maybe you’re just seeing things.
    -> floor_counter

=== floor_counter ===
#speaker:Narrator
The elevator ticks past the 4th floor. You’re close. Again.
    *   [Grip the knife.] -> knife_decision
    *   [Keep your hands at your sides.] -> restraint_path



= knife_decision 
You slide your hand into your pocket. The familiar shape is still there. Cold. Metal.
    *   [Pull it out. Fast.]  -> prepare_to_stab
    *   [Don’t move. Not this time.] -> choose_not_to_stab


= restraint_path 
//this path leads to End D Delusion or End A Innocent
~ refuseToStab = true
~ raise(trust)
#speaker:Narrator
You keep your fists clenched. But you don’t reach for the knife.
    -> elevator_ends
    


=== prepare_to_stab ===
//this path leads to End B Justified or End C Monster depending on delusion level

~ stabbed = true
You lunge. The same blur. The same sound. But this time—he’s ready.
#speaker:Male
“You again. You really don’t learn.”

    *   [Finish the stab.] -> stab_scene
    *   [Hesitate.] -> hesitation

= hesitation 
//this path leads to  End D Denial or End A Innocent depnding on delusion level

#speaker:Narrator
Your hand shakes. He grabs your wrist.
#speaker: Male
“You don’t have to keep doing this.”

    -> stab_struggle

= stab_scene 
#speaker:Narrator
Blood. Again. But this time, it’s more real. You swear it *hurts* you too.
    -> elevator_ends

= stab_struggle 
#speaker:Narrator
You drop the knife. He lets go. You both just stare at each other, breathing hard. No words.
    -> elevator_ends

=== choose_not_to_stab ===
//this path is available if the delusion is low
//this path leads to End B Justified or End A Innocent depending on trust

~ refuseToStab = true
~raise(trust)
#speaker:Narrator
You let go of the knife. You look him in the eye. He tilts his head.
#speaker:Male
“...Interesting.”

    *   [Say nothing.]
        -> elevator_ends
    *   [“You don’t deserve to die. Yet.”]
        ~ raise(delusion)
        -> elevator_ends
    *   [“I’m done playing this game.”]
        ~ lower(delusion)
        -> elevator_ends



=== elevator_ends ===
#speaker:Narrator
The elevator doors open. Same floor. Different you.

->DONE



-> SPLIT_ENDING


=== SPLIT_ENDING ===
{
    - up(trust) && down(delusion):
        -> END_A_INNOCENT // The Innocent – she was just scared, not wrong
        
    - down(trust) && down(delusion):
        -> END_B_JUSTIFIED // The Justified – she stopped a killer, but violently

    - down(trust) && up(delusion):
        -> END_C_MONSTER // The Monster – she’s mentally unstable and violent

    - up(trust) && up(delusion):
        -> END_D_DENIAL // Denial – she let her delusions lead her to trust evil

    - else:
        -> END_NEUTRAL // Optional fallback ending??
}





// === ROUND 4 START ===

=== END_A_INNOCENT
#pause_scene
#show_scene("elevator_bloodstained")
#set_expression("numb")

You step into the elevator again.

But this time... something’s wrong.

The light flickers red for a split second. Dried blood stains the metal wall behind you.

Your chest tightens.

#pause(1.2)

The elevator doors begin to close—  
—but they hesitate. Almost as if the system itself is… unsure.

Then, he enters.
->describe_man_round4

= describe_man_round4 
# show_character("Man", "neutral_calm")

His smile is faint. Almost pitying.

There’s a new scar across his cheek. A thin line. A memory.

#speaker:Male
"Back again."

#speaker:Narrator
You swallow hard. The air feels thick, like breathing through gauze.

#speaker:Female
*   [guarded] "Why do I always end up here?"
*   [guilt-ridden]  "I-I didn’t mean to hurt you last time…"
*   [numb] #speaker: Narrator 
<>You said nothing. Just stare.

-
#speaker:Male  
(slowly)  
"Funny, isn’t it? How cycles repeat. Even broken things can loop."
#speaker:Narrator
He says as he points towards the screen. 

-> elevator_ride_round4

= elevator_ride_round4 

#speaker:Narrator
The elevator descends—only, the numbers are wrong. They tick **up**, then **back down**, then flickers out completely.

You watch him in the mirrored wall. But something’s wrong—  
His reflection doesn't blink when he does.

Your stomach knots.

#speaker:Male
"You know, they told me you weren’t well."

#speaker:Female
*   [confused]
    "Who did?"
*   [angry]
    "They don’t know anything about me!"
*   [guilt]  
    "Maybe they’re right…"
-
#speaker:Male
(smiling thinly)  
"You didn’t see me clearly… but that wasn’t your fault. Your mind’s a noisy place. Full of smoke and broken glass."

-> self_doubt_builds

= self_doubt_builds 
#show_expression("tremble")

#speaker:Narrator
Your hand itches. You touch your bag—  
—but freeze.

#colour in red
There's no knife this time.

Only cold air.

Your vision blurs. You blink—  
—for a moment, the man’s skin seems to flicker. Pale and clammy. Lips split with blood. Eyes glassy. Dead.

You blink again.

He’s alive. Calm. Watching.

#speaker:Male
"What do you see when you look at me?"

#speaker:Female
*   [honest]  "I don’t know anymore. You’re… not what I thought."
*   [angry]  "You’re a liar. You’ve always been lying."
*   [scared] "I thought you were dangerous. But now I think I’m wrong."

-

#speaker:Male
#(whispering)
"..."
"Sometimes-"
"the greatest danger..."
"is the one holding the blade."

-> emotional_breakdown

= emotional_breakdown 
# show_expression("breaking")

You grip the rail. Knuckles white.  
Everything in your head is screaming.

You see flashes:  
Blood-soaked hands. Sirens.  
The man gasping on the floor.  
Your reflection laughing when you weren’t.

You fall to your knees.

+   [desperate] "W-What have I done...?"
+   [pleading]  "I didn’t want to hurt anyone…"
+   [lost]"Am I even real?!"
-
#speaker:Male
"You’re real enough."

-> final_confrontation

= final_confrontation 
#speaker:Narrator
...
Then elevator stops.  
The lights flicker violently.

The man steps closer. 
Too close.

You back up—  
—but the wall meets you. There’s nowhere left.

#speaker:Male
"You let your gut guide you. But guts lie, don’t they?"

"They twist fear into truth. And truth into knives."

#speaker:Narrator
He leans in.

#speaker:Male  
"But what if you were right the first time?"

#speaker:Female

*   [shaking]
    "What do you…?"
*   [tense]
    "Stop talking like that…"
*   [resigned]  
    "Then… I’ll die knowing I deserved it."
-

#speaker:Narrator
His hand moves to his jacket.

He pulls out something silver.
# show_expression("terror")

You try to scream—  
but there’s no time.


->death_scene
= death_scene 
# show_effect("stab_sound")
#show_expression("pain_shock")

Pain erupts in your neck.  
Warmth spills down your collar.

Your knees buckle.

You collapse to the floor, the world spinning.

The man kneels beside you, tilting your face.

His expression is gentle.

#speaker:Male
"Shhh. You were never supposed to see the whole picture."

#speaker:Narrator
He strokes your hair as your vision dims.

#speaker:Male
"So sleep now."

#speaker:Narrator
The your vision turned black.

-> main_ending



= main_ending
# show_scene("elevator_blackout")
#play_audio("flatline_hum")

A newspaper headline flashes across the screen:

**"ELEVATOR KILLER CLAIMS FINAL VICTIM – Mentally Unstable Woman Dies in Final Encounter"**

Then static.

Then silence.

-> END





===END_B_JUSTIFIED ===

# (The elevator is silent. The air hums with tension. The woman’s hands twitch by her sides.)

WOMAN: (internal) This is it. The final loop. I don’t know how I know, but... it feels different this time.

WOMAN: (internal) The way he's standing. How still he is. As if he's waiting.

# (He looks at her—no smirk, no charm, just quiet.)

MAN: You’re quieter today.

WOMAN: (hesitant) Maybe I’m listening better now.

MAN: (soft chuckle) And what are you listening for?

WOMAN: (internal) He’s trying to bait me again. That voice—calm, measured, too careful. Like he’s stepping around landmines.

-> player_choice_listening

= player_choice_listening 
*   "The lies between your words." 
    -> listening_lies
*   "My own heartbeat." 
    -> listening_heartbeat
*   "The truth I've ignored too long." 
    -> listening_truth

= listening_lies 
WOMAN: The lies between your words.

MAN: (smiling) Ouch. That's cold.

WOMAN: I’ve had four rounds to learn your rhythm. The spaces you leave say more than what you say out loud.

-> continue_end


= listening_heartbeat 
WOMAN: My own heartbeat.

MAN: Huh. Introspective today.

WOMAN: I had to learn the hard way. Listening to my thoughts nearly killed me. Now I listen to what’s real.

-> continue_end

= listening_truth 

#speaker:Female
"The truth I’ve ignored too long." 

#speaker: Male
(leans closer) "And what truth is that?"

#speaker:Female
"That I can be wrong."
"And that you’re not who I made you out to be." 
"..."
"You’re worse."

-> continue_end
= continue_end 

# speaker:Narrator
Suddenly, you saw something flickered at the edge of your sight.
Something that wasn't there before...
You inched your hand towards that thing, just a twitch at a time.

He’s waiting. Just like before. But this time... 
You're not frozen.

#speaker:Male
"You’re looking at me like you know something I don’t."

No. You're looking at him like you finally know what to believe.

#speaker:Male
"Why aren't you answering? What are you- !?"

-> emergency_brake

=emergency_brake 

#speaker:Narrator
You slammed the emergency brake.

# (Loud alarm. Lights flicker. The elevator shakes to a halt.)

#SFX: Emergency sirens blare.

He snaps. <>
#speaker:Male
"What the hell did you do?!"

#speaker: Female
I chose to believe in something real.

# speaker: Narrator
(Doors are pried open. Blinding flashlights flood in.)

POLICE OFFICER: Hands where we can see them!

(The man reaches into his coat—pulling something silver—)

POLICE OFFICER: DROP IT!

(He hesitates… then drops the object. It clatters. A *scalpel*.)

The sight of it gave you chills.
#speaker:Female
(breathing hard) It was real… all of it.

You helped out. Your legs nearly give. 
She looks back—
he stares at her, no longer smiling.

-> aftermath_awake

= aftermath_awake 

# (Black screen. Soft piano music.)

#speaker:Narrator
Later, in a quiet hospital corridor—

#speaker:Female
THERAPIST (V.O.): You did the right thing. You broke the pattern.

THERAPIST (V.O.): The delusions will try to return. But so will the truth, if you let it.

And so you left the room.
You run your hand along the wall as you walk through the hallway, grounding yourself.

And when you reach the end of the hallway, you saw an elevator, and the emergency exit next to it. 
*[Take the elevator] No, you will never enter that shit ever again in your life time.
+[Take the mergency exit]You took the emergency exit. There's a lot of staircase. 
-
But you don't care. 

So you stepped in.

#(Switch to cctv, this time, in the emergency exit. Shows a woman walked in through the door and took the staircase to go downstairs. And this time - a girl follows her. The girl noticed the cctv, did a 'shhh' gesture, and broke the cctv. Story ends.


-> END



=== END_C_MONSTER ===

# scene_elevator_end_round_4

# (The elevator is nearly at the top. The hum is louder now. She’s staring at the man again—trembling, but not from fear.)

WOMAN: (internal) I know what I saw. I know what he is.

MAN: You're looking at me like you want to say something.

WOMAN: Maybe I do.

MAN: Well, speak your truth.

WOMAN: (internal) Truth. Heh. They keep asking me what’s real. But none of them were in this elevator.

-> player_choice_truth

= player_choice_truth 
"You were going to kill me, weren’t you?" 

    -> delusion_kill_me
"You always smile when you lie." 

    -> delusion_smile_lie
"I saw you. In every version." 

    -> delusion_saw_you

= delusion_kill_me 
WOMAN: You were going to kill me, weren’t you?

MAN: (chuckles) That’s a bold claim.

WOMAN: I don’t need evidence. I remember the blood. I remember the fear.

-> continue_delusion

= delusion_smile_lie 
WOMAN: You always smile when you lie.

MAN: You’re imagining things.

WOMAN: That’s what they all say. But I see past it. I see you.

-> continue_delusion

= delusion_saw_you 
WOMAN: I saw you. In every version.

MAN: Maybe you dreamed me.

WOMAN: No. You were real. You always were. The knife. The blood. Your voice in the dark.

-> continue_delusion

= continue_delusion 

# (The hum of the elevator is rising in pitch. Her hand slides into her bag.)

WOMAN: (internal) They’ll never understand. They’ll think I’m mad again. That I need help.

WOMAN: But I don’t. I just need to end this.

MAN: What are you doing?

# (She pulls the knife out slowly. Her reflection in the metal gleams.)

WOMAN: Setting myself free.

# (She lunges forward—stabs—)

SFX: [Sickening crunch. Blood splatters the elevator wall.]

# (He slumps forward, gurgling, eyes wide.)

MAN: (choked whisper) You... were wrong...

# (The doors open. Screams. Police shouting.)

POLICE OFFICER: DROP THE KNIFE! DROP IT NOW!

# (But she just stands there. Covered in blood. Smiling.)

WOMAN: (softly) I’m finally awake.

-> aftermath_delusion

= aftermath_delusion 

# (Black screen. Soft ambient noise—muffled voices, flickering lights.)

NARRATOR: Days later. A padded room. A woman sits facing a mirror that doesn’t reflect her.

THERAPIST (V.O.): You don’t remember, do you?

WOMAN: I remember everything.

THERAPIST (V.O.): There was no man.

# (The woman turns to the mirror.)

WOMAN: He was real.

THERAPIST (V.O.): We found no one. No victim. Only you. Covered in blood.

WOMAN: (internal) They’re lying. Just like he did. Just like everyone does.

# (The mirror flickers. For just a second, the man’s face appears in the reflection—smiling.)

-> END





=== END_D_DENIAL ===



# (The elevator ride is slow. The lights flicker again. Her breathing is shallow, but steady.)

WOMAN: (internal) Maybe I was wrong. Maybe I imagined it. He hasn’t done anything.

# (The man stands at the far end, leaning against the rail. Calm. Watching her.)

MAN: You’re quiet today.

WOMAN: Just tired.

MAN: Tired of being afraid?

WOMAN: …Maybe.

-> denial_choice_approach

 = denial_choice_approach 
"I just want to get out of here." 

    -> denial_escape
"This is just in my head, right?" 

    -> denial_doubt
Stay silent. 

    -> denial_silent

= denial_escape 
WOMAN: I just want to get out of here.

MAN: (smiles) That makes two of us.

WOMAN: No more talking. Just let this be over.

-> continue_denial

= denial_doubt 
WOMAN: This is just in my head, right?

MAN: Does it matter?

WOMAN: It does to me.

MAN: Then pretend it is. Pretend everything’s fine.

-> continue_denial

= denial_silent 
# (She doesn't speak. Just clutches her bag tighter.)

MAN: Still pretending I’m not here?

WOMAN: (internal) If I don’t acknowledge him, he can’t hurt me. Right?

-> continue_denial

= continue_denial 

# (The elevator dings. Floor 14. Still going. The air is thick. She shifts her weight.)

WOMAN: (internal) One more minute. That’s all. Just get through one more minute.

MAN: You ever wonder what happens when we hit the top?

WOMAN: No.

MAN: You should.

# (Suddenly, the elevator *jerks violently*. Lights snap off. Emergency red light glows.)

SFX: [Mechanical groaning. A rising shriek in the metal.]

WOMAN: Wh-What’s happening?!

# (The man is gone.)

WOMAN: No. No no no no—

# (She turns — he’s now standing *right beside her*.)

MAN: You should’ve faced it.

WOMAN: What… what are you?

# (He smiles — mouth stretching impossibly wide. His eyes *bleed black*.)

MAN: Just a part of you.

# (She tries to scream. He places a finger on her lips.)

MAN: Shhh. This is your silence. You asked for it.

# (He plunges something into her chest. Not a knife — just *darkness*. It spreads like ink.)

SFX: [Heartbeat slows. The elevator doors open to nothing — a void.]

-> aftermath_denial

= aftermath_denial 

# (Black. Then faint static. A flickering video feed.)

NARRATOR (V.O.): Police investigation report. Elevator 17. No signs of struggle. No victim. No assailant.

DISPATCH (V.O.): Last known footage shows a woman standing alone. Smiling. Talking to herself.

# (The screen flickers to *CCTV footage* — she’s speaking, gesturing… to no one.)

WOMAN (CCTV): (murmuring) I just want to go home.

# (Static.)

NARRATOR (V.O.): No trace of her ever found.

# (Final flash: A red-lit elevator. Empty… except for a *bloody handprint* on the mirror.)

-> END





=== END_NEUTRAL ===
THIS IS NEUTRAL ENDING

->END
