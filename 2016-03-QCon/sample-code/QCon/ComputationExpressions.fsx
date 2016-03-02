// async workflows great for IO bound computations



//let asyncWF = async {    }


type ThrottleMessage = {
  Url: string
  Query: seq<string * string>
  Reply : AsyncReplyChannel<string>
}



// Async used in the context of an agent

type ComplexMessage = {
  Name: string
  Age: int
  Channel: AsyncReplyChannel<string>
  }

type EchoMessage = {
  Name: string
  Channel: AsyncReplyChannel<string>
  }

let echo = MailboxProcessor.Start(fun inbox ->
  async{
    while true do 
      let! message = inbox.Receive()
      message.Channel.Reply("Hello "+ message.Name)
  }
)

echo.PostAndAsyncReply( fun ch -> { EchoMessage.Name = "World"; EchoMessage.Channel= ch})