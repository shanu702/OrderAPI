# OrderAPI Project structures.

OrderApi/            #cs proj file.

  -- Controllers/            # Controller for handling API requests
      -- OrderController.cs
 
  -- Services/
      -- OrderQueueService.cs        # Service for managing order queue
      -- OrderProcessingService.cs  # Service for processing orders from the queue
      -- OrderProcessingBackgroundService.cs  # Background service to move orders from the queue to in-memory storage
   -- Middleware/
      -- ExceptionHandlingMiddleware.cs  # global Exception Handling.
   -- Validators/
      -- OrderRequestValidator.cs   # Validations for input
-- Program.cs                      # Startup file.
-- Startup.cs                      # Service & middleware configurations

 -- Order.Entities/         #cs proj file.
      ─ Order.cs                    # Order model
      -─ OrderRequest.cs            # Input Request Model.
      
-- OrderApi.Tests/            #cs proj file.
      -- OrderControllerTests.cs   #  UNIT TEST CASES
   

